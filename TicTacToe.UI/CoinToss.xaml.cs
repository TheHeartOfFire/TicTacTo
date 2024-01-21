using System.Windows;
using System.Windows.Media;
using static TicTacToe.UI.ThemeManager;

namespace TicTacToe.UI
{
    /// <summary>
    /// Interaction logic for CoinToss.xaml
    /// </summary>
    public partial class CoinToss : Window
    {

        public CoinToss()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Randomly assign the person who clicks the button to be player 1 or player 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCoinToss_Click(object sender, RoutedEventArgs e)
        {
            var rand = new Random();

            lblFirst.Content = "You go first!";
            imgIcon.Source = (ImageSource)ActiveTheme.ResDict["Player1"];

            var num = rand.NextDouble();//generates a number between 0 and 1

            if (Math.Round(num, MidpointRounding.AwayFromZero)>0)//0.5 and up rounds to 1, everything else rounds to 0.
            {
                lblFirst.Content = "You go second!";
                imgIcon.Source = (ImageSource)ActiveTheme.ResDict["Player2"];
            }

            btnCoinToss.Visibility = Visibility.Hidden;
            imgIcon.Visibility = Visibility.Visible;
        }
    }
}
