using System.Windows;
using System.Windows.Controls;
using TicTacToe.AI;
using TicTacToe.Core;
using TicTacToe.UI.EventArgs;

namespace TicTacToe.UI.Controls
{
    /// <summary>
    /// Interaction logic for TicTacToeDisplay.xaml
    /// </summary>
    public partial class TicTacToeDisplay : UserControl
    {
        public readonly TicTacToeBoard board;

        public TicTacToeDisplay(int size)
        {
            InitializeComponent();
            board = new TicTacToeBoard(size);
            Init();
        }
        public TicTacToeDisplay()
        {
            InitializeComponent();
            board = new TicTacToeBoard(3);
            Init();
        }
        public void Reset (int? size)
        {

            btnReset.Visibility = Visibility.Hidden;//Hide the reset button. This should only be visible at the end of the game.

            lblAlert.Content = "";//This label is only visible at the end of the game.

            if (BotManager.Instance.Bot is not null)
            {

                lblAlert.Content = "Your turn!";
                if ((board.Turn is Tile.TileOwner.Player1 && BotManager.Instance.Bot.Order is Tile.TileOwner.Player1)
                     || (board.Turn is Tile.TileOwner.Player2 && BotManager.Instance.Bot.Order is Tile.TileOwner.Player2))
                    lblAlert.Content = "Opponent's turn!";
            }
            board.Reset(size);
        }

        public void Dispose()
        {

            board.GameEnded -= TicTacToeBoard_GameEnded;
            board.PlayerTurnOver -= Board_PlayerTurnOver;
            board.StalemateImminent -= Board_StalemateImminent;

            board.Dispose();

            BotManager.Instance.BotChanged -= Instance_BotChanged;
            if (BotManager.Instance.Bot is not null)
                BotManager.Instance.Bot.TurnOver -= Bot_TurnOver;
        }
        private void Init()
        {
            grdBoard.Children.Add(board);
            Grid.SetRow(board, 1);
            board.VerticalAlignment = VerticalAlignment.Stretch;
            board.HorizontalAlignment = HorizontalAlignment.Stretch;
            board.GameEnded += TicTacToeBoard_GameEnded;
            board.PlayerTurnOver += Board_PlayerTurnOver;
            board.StalemateImminent += Board_StalemateImminent;

            BotManager.Instance.BotChanged += Instance_BotChanged;
        }

        private void Board_StalemateImminent()
        {
            lblAlert.Content = "Stalemate!";
        }

        private void Board_PlayerTurnOver()
        {
            if (BotManager.Instance.Bot is not null)
                lblAlert.Content = "Opponent's turn!";
        }

        private void Bot_TurnOver(object sender, AI.EventArgs.TurnOverEventArgs e)
        {
            lblAlert.Content = "Your turn!";
        }

        private void Instance_BotChanged()
        {
            lblAlert.Content = string.Empty;
            if(BotManager.Instance.Bot is not null)
            {
                BotManager.Instance.Bot.TurnOver += Bot_TurnOver;

                lblAlert.Content = "Your turn!";
                if (    (board.Turn is Tile.TileOwner.Player1 && BotManager.Instance.Bot.Order is Tile.TileOwner.Player1)
                     || (board.Turn is Tile.TileOwner.Player2 && BotManager.Instance.Bot.Order is Tile.TileOwner.Player2))
                    lblAlert.Content = "Opponent's turn!";
            }
        }

        private void TicTacToeBoard_GameEnded(object sender, GameOverEventArgs e)
        {
            lblAlert.Content = "Winner!";

            if (e.Result.Winner is WinResult.WinType.Stalemate)//If the game was decisive, Tell the player there was a winner, otherwise, tell them it was a stalemate.
                lblAlert.Content = "Stalemate!";

            if (BotManager.Instance.Bot is not null)
            {
                lblAlert.Content = "You Win!";
                if (    (e.Result.Winner is WinResult.WinType.Player1 && BotManager.Instance.Bot.Order is Tile.TileOwner.Player1) 
                    ||  (e.Result.Winner is WinResult.WinType.Player2 && BotManager.Instance.Bot.Order is Tile.TileOwner.Player2))
                    lblAlert.Content = "Opponent wins!";
            }

            btnReset.Visibility = Visibility.Visible;//Allow the players to start a new game
        }

        private void btnReset_Click(object sender, RoutedEventArgs e) => Reset(null);
    }
}
