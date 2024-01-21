using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe.UI.Controls
{
    /// <summary>
    /// Interaction logic for TicTacToeTile.xaml
    /// </summary>
    public partial class TicTacToeTile : UserControl
    {
        public readonly int Index;
        private readonly Action<TicTacToeTile> takeTurn;
        public TicTacToeTile(int index, Action<TicTacToeTile> takeTurn)
        {
            InitializeComponent();
            Index = index;
            this.takeTurn = takeTurn;
        }

        public void UpdateImage(ImageSource image) => imgDisplay.Source = image;
        private void btnControl_Click(object sender, RoutedEventArgs e) => takeTurn(this);

    }
}
