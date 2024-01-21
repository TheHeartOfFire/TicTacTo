using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using TicTacToe.AI;
using TicTacToe.Core;
using TicTacToe.UI.EventArgs;
using static TicTacToe.Core.Tile;
using static TicTacToe.UI.ThemeManager;
using Image = System.Windows.Controls.Image;

namespace TicTacToe.UI.Controls
{
    /// <summary>
    /// Interaction logic for TicTacToeBoard.xaml
    /// </summary>
    public partial class TicTacToeBoard : UserControl
    {

        public delegate void GameOverEventHandler(object sender, GameOverEventArgs e);
        public delegate void Notify();

        public event GameOverEventHandler? GameEnded;
        public event Notify? StalemateImminent;
        public event Notify? PlayerTurnOver;
        protected virtual void OnGameOver(GameOverEventArgs e) => GameEnded?.Invoke(this, e);
        protected virtual void OnStalemateImminent() => StalemateImminent?.Invoke();
        protected virtual void OnPlayerTurnOver()=>PlayerTurnOver?.Invoke();

        public TileOwner Turn 
        { 
            get
            {
                return player1 ? TileOwner.Player1 : TileOwner.Player2;
            } 
        }
        public int Size => size;
        private Board game = new();
        private bool player1 = true;
        private TicTacToeTile[]? tiles;
        private int size = 3;
        public TicTacToeBoard(int size)
        {
            Reset(size);
            ActiveTheme.ThemeChanged += ActiveTheme_ThemeChanged;
            BotManager.Instance.BotChanged += Instance_BotChanged;
        }

        public TicTacToeBoard()
        {
            Reset(3);
            ActiveTheme.ThemeChanged += ActiveTheme_ThemeChanged;
            BotManager.Instance.BotChanged += Instance_BotChanged;
        }
        public void Reset(int? size)
        {
            this.size = size??this.size;

            game = new(this.size);//Create a new game board to play on

            grdContent?.Children.Clear();
            InitializeComponent();
            DefineRowsAndCols();
            GenerateDividers();
            tiles = GenerateTiles();

            Cursor = ActiveTheme.Player1Cursor;//reset the cursor

            player1 = true;
            if (BotManager.Instance.Bot is not null && BotManager.Instance.Bot.Order is TileOwner.Player1)
                BotManager.Instance.Bot.TakeTurn(game, null);
        }
        public void Dispose()
        {
            ActiveTheme.ThemeChanged -= ActiveTheme_ThemeChanged;

            if (BotManager.Instance.Bot is not null)
                BotManager.Instance.Bot.TurnOver -= Bot_TurnOver;
        }

        private void Bot_TurnOver(object sender, AI.EventArgs.TurnOverEventArgs e)
        {
            if (tiles is null) return;

            var tile = tiles[e.TurnTaken.Index];
            tile.UpdateImage((ImageSource)ActiveTheme.ResDict[e.TurnTaken.Owner is TileOwner.Player1 ? "Player1" : "Player2"]);
            tile.btnControl.IsEnabled = false;
            UpdatePlayer();
        }

        private void ActiveTheme_ThemeChanged(object? sender, System.EventArgs e)
        {
            if (sender is not ThemeManager manager) return;

            manager.ThemeChanged -= ActiveTheme_ThemeChanged;
            ActiveTheme.ThemeChanged += ActiveTheme_ThemeChanged;
            ChangeTheme();
        } 


        private void Instance_BotChanged()
        {
            if (BotManager.Instance.Bot is null) return;

            BotManager.Instance.Bot.TurnOver += Bot_TurnOver;

            Reset(null);
        }

        /// <summary>
        /// Check for a win condition
        /// </summary>
        private void GameOver()
        {
            var winner = game.CheckForWin();//Get the current WinStatus of the game
            if (winner.Winner is WinResult.WinType.None) return;//If the game is still in progress, do nothing

            Cursor = Cursors.Arrow;//Change the cursor back to the normal one as no players are taking a turn

            DisplayWinner(winner);//Display the results of the game
            OnGameOver(new GameOverEventArgs(winner));

        }
        /// <summary>
        /// Display the results of a game that has ended
        /// </summary>
        /// <param name="result"></param>
        private void DisplayWinner(WinResult result)
        {


            foreach(var tile in tiles)
            {
                tile.btnControl.IsEnabled = false;//disable any remaining buttons

                if (result.Winner is WinResult.WinType.Stalemate)
                    tile.imgDisplay.Source = (ImageSource)ActiveTheme.ResDict["Stalemate"];//if the game was a stalemate, fill all tiles with stalemate image

                if (!game.Positions[tile.Index].WinningTile && result.Winner is not WinResult.WinType.Stalemate)
                    tile.imgDisplay.Visibility = Visibility.Hidden;//if the game was decisive, show the tiles that make up the win condition

            }
        }
        /// <summary>
        /// If the current player is player 1, change the current player to player 2 or vise versa
        /// </summary>
        private void UpdatePlayer()
        {
            player1 = !player1;//Alternate which player is currently playing
            Cursor = player1 ? ActiveTheme.Player1Cursor : ActiveTheme.Player2Cursor;//Update the cursor to match the current player
        }

        /// <summary>
        /// Process a single turn for specific tile
        /// </summary>
        /// <param name="img"></param>
        /// <param name="btn"></param>
        /// <param name="pos"></param>
        private void TakeTurn(TicTacToeTile tile)
        {
            game.TakeTurn(player1 ? TileOwner.Player1 : TileOwner.Player2, tile.Index);//process the turn
            tile.UpdateImage( player1 ? (ImageSource)ActiveTheme.ResDict["Player1"] : (ImageSource)ActiveTheme.ResDict["Player2"]);//set the tile's image to the icon for the current player

            tile.btnControl.IsEnabled = false;//disable the button so that this tile can't be chosen again this game
            UpdatePlayer();//update who the current player is
            GameOver();//Check for a win condition
            if(game.IsImminentStalemate) 
                OnStalemateImminent();

            OnPlayerTurnOver();

            if (BotManager.Instance.Bot is not null)
                BotManager.Instance.TakeTurn(game, game.Positions[tile.Index]);
        }

        private void ChangeTheme()
        {
            if (tiles is null || tiles.Length <= 0 ) return;

            foreach(var tile in tiles)
            {
                if (game.Positions[tile.Index].Owner is not TileOwner.Unclaimed)
                    tile.imgDisplay.Source = (ImageSource)ActiveTheme.ResDict[game.Positions[tile.Index].Owner.ToString()];

                if (game.CheckForWin().Winner is WinResult.WinType.Stalemate)
                    tile.imgDisplay.Source = (ImageSource)ActiveTheme.ResDict["Stalemate"];
            }
            Cursor = player1 ? ActiveTheme.Player1Cursor : ActiveTheme.Player2Cursor;//update cursor
        }
        

        private TicTacToeTile[] GenerateTiles()
        {
            var tiles = new TicTacToeTile[size * size];

            for(int i = 0; i < tiles.Length; i++)
            {
                var tile = new TicTacToeTile(i, TakeTurn);

                grdContent.Children.Add(tile);

                Grid.SetRow(tile, i/size);
                Grid.SetColumn(tile, i%size);

                tiles[i] = tile;
            }

            return tiles;
        }

        private void DefineRowsAndCols()
        {
            grdContent.ColumnDefinitions.Clear();
            grdContent.RowDefinitions.Clear();
            for(int i = 0; i<size; i++)
            {
                grdContent.ColumnDefinitions.Add(new ColumnDefinition());
                grdContent.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void GenerateDividers()
        {
            for (int i = 0; i < size-1; i++)
            {
                var vertical = new Image
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var vertHeighBinding = new Binding("ActualHeight")
                {
                    Source = grdContent
                };
                vertical.SetBinding(HeightProperty, vertHeighBinding);
                vertical.SetResourceReference(Image.SourceProperty, "VerticalDivider");
                var horizontal = new Image
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var horizWidthBinding = new Binding("ActualWidth")
                {
                    Source = grdContent
                };
                vertical.SetBinding(WidthProperty, horizWidthBinding);
                horizontal.SetResourceReference(Image.SourceProperty, "HorizontalDivider");

                Grid.SetRow(horizontal, i);
                Grid.SetColumnSpan(horizontal, size);
                Grid.SetRowSpan(horizontal, 2);

                Grid.SetColumn(vertical, i);
                Grid.SetRowSpan(vertical, size);
                Grid.SetColumnSpan(vertical, 2);

                grdContent.Children.Add(vertical);
                grdContent.Children.Add(horizontal);
            }
        }
    }
}
