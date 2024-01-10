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
        private readonly Button[] buttons;
        private readonly Image[] images;
        private ThemeManager theme;
        public TicTacToeBoard()
        {
            InitializeComponent();
            buttons = [btnPos0, btnPos1, btnPos2, btnPos3, btnPos4, btnPos5, btnPos6, btnPos7, btnPos8];
            images = [imgPos0, imgPos1, imgPos2, imgPos3, imgPos4, imgPos5, imgPos6, imgPos7, imgPos8];

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
            foreach (var btn in buttons)
                btn.IsEnabled = false;//disable any remaining buttons

            for (int i = 0; i < images.Length; i++)
            {
                if (!result.WinningTileIndicies.Contains(i) && result.Winner is not WinResult.WinType.Stalemate)
                    images[i].Visibility = Visibility.Hidden;//if the game was decisive, show the tiles that make up the win condition

                

                if (result.Winner is WinResult.WinType.Stalemate)
                    images[i].Source = (ImageSource)theme.ResDict["Stalemate"];//if the game was a stalemate, fill all tiles with stalemate image
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
        private void TakeTurn(Image img, Button btn, int pos)
        {
            game.TakeTurn(player1 ? TileOwner.Player1 : TileOwner.Player2, pos);//process the turn
            img.Source = player1 ? (ImageSource)theme.ResDict["Player1"] : (ImageSource)theme.ResDict["Player2"];//set the tile's image to the icon for the current player

            btn.IsEnabled = false;//disable the button so that this tile can't be chosen again this game
            UpdatePlayer();//update who the current player is
            GameOver();//Check for a win condition
        }


        //Tile buttons
        private void btnPos0_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos0, btnPos0, 0);
        private void btnPos1_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos1, btnPos1, 1);
        private void btnPos2_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos2, btnPos2, 2);
        private void btnPos3_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos3, btnPos3, 3);
        private void btnPos4_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos4, btnPos4, 4);
        private void btnPos5_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos5, btnPos5, 5);
        private void btnPos6_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos6, btnPos6, 6);
        private void btnPos7_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos7, btnPos7, 7);
        private void btnPos8_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos8, btnPos8, 8);


        /// <summary>
        /// Applies the specified theme to the window
        /// </summary>
        /// <param name="themeType"></param>
        public void ChangeTheme(Theme themeType) => ChangeTheme(themeType, out theme);

        private void ChangeTheme(Theme themeType, out ThemeManager theme)
        {
            theme = GetTheme(themeType);

            for (int i = 0; i < game.Positions.Length; i++)//update tiles
            {
                if (game.Positions[i].Owner is TileOwner.Player1)
                    images[i].Source = (ImageSource)theme.ResDict["Player1"];
                if (game.Positions[i].Owner is TileOwner.Player2)
                    images[i].Source = (ImageSource)theme.ResDict["Player2"];
                if (game.CheckWin().Winner is WinResult.WinType.Stalemate)
                    images[i].Source = (ImageSource)theme.ResDict["Stalemate"];
            }

            Cursor = player1 ? theme.Player1Cursor : theme.Player2Cursor;//update cursor

            imgVerticalDivider0.Visibility = theme.UseBorders ? Visibility.Visible : Visibility.Hidden;//All borders' visibility are databound to this one.
        }
        
        public void Reset()
        {
            foreach (var btn in buttons)//Turn all fo the buttons back on
            {

                btn.Visibility = Visibility.Visible;
                btn.IsEnabled = true;
            }
            foreach (var img in images)//Clear the images
            {
                img.Source = null;
                img.Visibility = Visibility.Visible;
            }

            Cursor = theme.Player1Cursor;//reset the cursor

            game = new();//Create a new game board to play on
            player1 = true;
        }
    }
}
