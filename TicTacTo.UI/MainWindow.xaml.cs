using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TicTacTo.Core;

namespace TicTacTo.UI;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Board game = new();
    private bool player1 = true;
    private readonly Button[] buttons;
    private readonly Image[] images;

    public MainWindow()
    {
        InitializeComponent();
        buttons = [btnPos0,
            btnPos1,
            btnPos2,
            btnPos3,
            btnPos4,
            btnPos5,
            btnPos6,
            btnPos7,
            btnPos8];
        images = [imgPos0,
            imgPos1,
            imgPos2,
            imgPos3,
            imgPos4,
            imgPos5,
            imgPos6,
            imgPos7,
            imgPos8];

    }

    private void GameOver()
    {
        var winner = game.CheckWin();
        if (winner < 0) return;
        lblDisplay.Content = "Winner";

        string img = Properties.Resources.Ladybug;
        if (winner == 1) img = Properties.Resources.Butterfly;
        if (winner == 2) img = Properties.Resources.Bird;


        imgIndicator.Source = new BitmapImage(new Uri(img, UriKind.Relative));

        foreach (var button in buttons) button.Visibility = Visibility.Hidden;

        btnReset.Visibility = Visibility.Visible;
    }

    private void UpdatePlayer()
    {
        player1 = !player1;
        BitmapImage img = new BitmapImage(new Uri(player1 ? Properties.Resources.Ladybug : Properties.Resources.Butterfly, UriKind.Relative));
        imgIndicator.Source = img;
    }

    private void TakeTurn(Image img, Button btn, int pos)
    {
        game.TakeTurn(player1 ? 0 : 1, pos);
        img.Source = new BitmapImage(new Uri(player1 ? Properties.Resources.Ladybug : Properties.Resources.Butterfly, UriKind.Relative));

        btn.IsEnabled = false;
        UpdatePlayer();
        GameOver();
    }



    private void btnPos0_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos0, btnPos0, 0);
    private void btnPos1_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos1, btnPos1, 1);
    private void btnPos2_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos2, btnPos2, 2);
    private void btnPos3_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos3, btnPos3, 3);
    private void btnPos4_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos4, btnPos4, 4);
    private void btnPos5_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos5, btnPos5, 5);
    private void btnPos6_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos6, btnPos6, 6);
    private void btnPos7_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos7, btnPos7, 7);
    private void btnPos8_Click(object sender, RoutedEventArgs e) => TakeTurn(imgPos8, btnPos8, 8);
    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        btnReset.Visibility = Visibility.Hidden;

        foreach (var btn in buttons)
        {

            btn.Visibility = Visibility.Visible;
            btn.IsEnabled = true;
        }
        foreach (var img in images) img.Source = null;

        lblDisplay.Content = "Turn";
        imgIndicator.Source = new BitmapImage(new Uri(Properties.Resources.Ladybug, UriKind.Relative));

        game = new();
        player1 = true;
    }

}