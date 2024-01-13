using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TicTacToe.Core;
using TicTacToe.UI.EventArgs;
using static System.Net.Mime.MediaTypeNames;
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

        public event GameOverEventHandler? GameEnded;
        protected virtual void OnGameOver(GameOverEventArgs e) => GameEnded?.Invoke(this, e);

        private Board game = new();
        private bool player1 = true;
        private ThemeManager theme = GetTheme(Theme.BUG);
        private readonly TicTacToeTile[] tiles;
        private readonly int size = 3;
        public TicTacToeBoard(int size)
        {
            this.size = size;
            game = new(size);
            InitializeComponent();
            DefineRowsAndCols();
            GenerateDividers();
            tiles = GenerateTiles();

            ChangeTheme(Theme.BUG, out theme);
        }
        public TicTacToeBoard()
        {
            game = new(size);
            InitializeComponent();
            DefineRowsAndCols();
            GenerateDividers();
            tiles = GenerateTiles();

            ChangeTheme(Theme.BUG, out theme);
        }

        /// <summary>
        /// Check for a win condition
        /// </summary>
        private void GameOver()
        {
            var winner = game.CheckWin();//Get the current WinStatus of the game
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
                    tile.imgDisplay.Source = (ImageSource)theme.ResDict["Stalemate"];//if the game was a stalemate, fill all tiles with stalemate image

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
            Cursor = player1 ? theme.Player1Cursor : theme.Player2Cursor;//Update the cursor to match the current player
        }

        /// <summary>
        /// Process a single turn for specific tile
        /// </summary>
        /// <param name="img"></param>
        /// <param name="btn"></param>
        /// <param name="pos"></param>
        private void TakeTurn(TicTacToeTile tile, int pos)
        {

            game.TakeTurn(player1 ? TileOwner.Player1 : TileOwner.Player2, pos);//process the turn
            tile.UpdateImage( player1 ? (ImageSource)theme.ResDict["Player1"] : (ImageSource)theme.ResDict["Player2"]);//set the tile's image to the icon for the current player

            tile.btnControl.IsEnabled = false;//disable the button so that this tile can't be chosen again this game
            UpdatePlayer();//update who the current player is
            GameOver();//Check for a win condition
        }

        /// <summary>
        /// Applies the specified theme to the window
        /// </summary>
        /// <param name="themeType"></param>
        public void ChangeTheme(Theme themeType) => ChangeTheme(themeType, out theme);

        private void ChangeTheme(Theme themeType, out ThemeManager theme)
        {
            theme = GetTheme(themeType);

            foreach(var tile in tiles)
            {
                if (game.Positions[tile.Index].Owner is not TileOwner.Unclaimed)
                    tile.imgDisplay.Source = (ImageSource)theme.ResDict[game.Positions[tile.Index].Owner.ToString()];

                if (game.CheckWin().Winner is WinResult.WinType.Stalemate)
                    tile.imgDisplay.Source = (ImageSource)theme.ResDict["Stalemate"];
            }


            Cursor = player1 ? theme.Player1Cursor : theme.Player2Cursor;//update cursor

        }
        
        public void Reset()
        {
            foreach(var tile in tiles)
            {
                tile.btnControl.Visibility = Visibility.Visible;
                tile.btnControl.IsEnabled = true;
                tile.imgDisplay.Source = null;
                tile.imgDisplay.Visibility = Visibility.Visible;
            }

            Cursor = theme.Player1Cursor;//reset the cursor

            game = new(size);//Create a new game board to play on
            player1 = true;
        }

        private TicTacToeTile[] GenerateTiles()
        {
            var tiles = new TicTacToeTile[size * size];

            for(int i = 0; i < tiles.Length; i++)
            {
                var tile = new TicTacToeTile(i, TakeTurn);

                grdContent.Children.Add(tile);

                Grid.SetRow(tile, i%size);
                Grid.SetColumn(tile, i/size);

                tiles[i] = tile;
            }

            return tiles;
        }

        private void DefineRowsAndCols()
        {
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
                    Source = (ImageSource)theme.ResDict["VerticalDivider"],
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Height = double.NaN,
                    Width = double.NaN
                };
                var horizontal = new Image
                {
                    Source = (ImageSource)theme.ResDict["HorizontalDivider"],
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = double.NaN,
                    Width = double.NaN
                };

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
