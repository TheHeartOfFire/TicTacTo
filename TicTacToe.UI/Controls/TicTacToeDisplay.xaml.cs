using System;
using System.Collections.Generic;
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
using static TicTacToe.UI.ThemeManager;

namespace TicTacToe.UI.Controls
{
    /// <summary>
    /// Interaction logic for TicTacToeDisplay.xaml
    /// </summary>
    public partial class TicTacToeDisplay : UserControl
    {
        public TicTacToeDisplay()
        {
            InitializeComponent();

            ticTacToeBoard.GameEnded += TicTacToeBoard_GameEnded;
        }

        private void TicTacToeBoard_GameEnded(object sender, GameOverEventArgs e)
        {
            lblAlert.Content = "Winner!";

            if (e.Result.Winner is WinResult.WinType.Stalemate)//If the game was decisive, Tell the player there was a winner, otherwise, tell them it was a stalemate.
                lblAlert.Content = "Stalemate!";

            btnReset.Visibility = Visibility.Visible;//Allow the players to start a new game
        }

        public void ChangeTheme(Theme theme)
        {
            ticTacToeBoard.ChangeTheme(theme);
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            btnReset.Visibility = Visibility.Hidden;//Hide the reset button. This should only be visible at the end of the game.

            lblAlert.Content = "";//This label is only visible at the end of the game.

            ticTacToeBoard.Reset();
            
        }
    }
}
