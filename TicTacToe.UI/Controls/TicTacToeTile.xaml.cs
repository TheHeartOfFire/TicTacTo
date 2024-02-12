using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static TicTacToe.Core.Tile;

namespace TicTacToe.UI.Controls
{
    /// <summary>
    /// Interaction logic for TicTacToeTile.xaml
    /// </summary>
    public partial class TicTacToeTile : UserControl
    {
        public readonly Coordinates Coords;
        private readonly Action<TicTacToeTile> takeTurn;
        public TicTacToeTile(Coordinates coords, Action<TicTacToeTile> takeTurn)
        {
            InitializeComponent();
            Coords = coords;
            this.takeTurn = takeTurn;
        }

        public void UpdateImage(ImageSource? image) => imgDisplay.Source = image;
        private void btnControl_Click(object sender, RoutedEventArgs e) => takeTurn(this);

    }
}
