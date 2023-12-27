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
using System.Windows.Shapes;
using static TicTacToe.UI.ThemeManager;

namespace TicTacToe.UI
{
    /// <summary>
    /// Interaction logic for CoinToss.xaml
    /// </summary>
    public partial class CoinToss : Window
    {
        private ThemeManager Theme;

        public CoinToss(ThemeManager theme)
        {
            Theme = theme;
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
            imgIcon.Source = (ImageSource)Theme.ResDict["Player1"];

            var num = rand.NextDouble();//generates a number between 0 and 1

            if (Math.Round(num, MidpointRounding.AwayFromZero)>0)//0.5 and up rounds to 1, everything else rounds to 0.
            {
                lblFirst.Content = "You go second!";
                imgIcon.Source = (ImageSource)Theme.ResDict["Player2"];
            }

            btnCoinToss.Visibility = Visibility.Hidden;
            imgIcon.Visibility = Visibility.Visible;
        }
    }
}
