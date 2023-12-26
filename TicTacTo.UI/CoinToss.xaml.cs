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
using static TicTacTo.UI.ThemeManager;

namespace TicTacTo.UI
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

        private void btnCoinToss_Click(object sender, RoutedEventArgs e)
        {
            var rand = new Random();

            lblFirst.Content = "You go first!";
            imgIcon.Source = (ImageSource)Theme.ResDict["Player1"];

            var num = rand.NextDouble();

            if (Math.Round(num, MidpointRounding.AwayFromZero)>0)
            {
                lblFirst.Content = "You go second!";
                imgIcon.Source = (ImageSource)Theme.ResDict["Player2"];
            }
            btnCoinToss.Visibility = Visibility.Hidden;
            lblPress.Visibility = Visibility.Hidden;
            lblIcon.Visibility = Visibility.Visible;
            lblFirst.Visibility = Visibility.Visible;
            imgIcon.Visibility = Visibility.Visible;
        }
    }
}
