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

namespace TicTacToe.UI.Controls
{
    /// <summary>
    /// Interaction logic for TicTacToeTile.xaml
    /// </summary>
    public partial class TicTacToeTile : UserControl
    {
        public readonly int Index;
        private readonly Action<TicTacToeTile, int> takeTurn;
        public TicTacToeTile(int index, Action<TicTacToeTile, int> takeTurn)
        {
            InitializeComponent();
            Index = index;
            this.takeTurn = takeTurn;
        }

        public void UpdateImage(ImageSource image) => imgDisplay.Source = image;
        private void btnControl_Click(object sender, RoutedEventArgs e) => takeTurn(this, Index);

    }
}
