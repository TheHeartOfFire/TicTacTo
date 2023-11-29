using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TicTacTo.Core;

namespace TicTacTo.UI;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Board game = new();
    private bool player1 = true;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void GameOver()
    {
        var winner = game.CheckWin();
        if (winner < 0) return;
        lblDisplay.Content = "Winner";

        string img = "Assets\\Ladybug.png";
        if (winner == 1) img = "Assets\\Bee.png";
        if (winner == 2) img = "Assets\\Bird.png";


        imgIndicator.Source = new BitmapImage(new Uri(img, UriKind.Relative));
        btnPos0.Visibility = Visibility.Hidden;
        btnPos1.Visibility = Visibility.Hidden;
        btnPos2.Visibility = Visibility.Hidden;
        btnPos3.Visibility = Visibility.Hidden;
        btnPos4.Visibility = Visibility.Hidden;
        btnPos5.Visibility = Visibility.Hidden;
        btnPos6.Visibility = Visibility.Hidden;
        btnPos7.Visibility = Visibility.Hidden;
        btnPos8.Visibility = Visibility.Hidden;
        btnReset.Visibility = Visibility.Visible;
    }

    private void UpdatePlayer()
    {
        player1 = !player1;
        imgIndicator.Source = new BitmapImage(new Uri(player1?"Assets\\Ladybug.png":"Assets\\Bee.png", UriKind.Relative));


    }



    private void btnPos0_Click(object sender, RoutedEventArgs e)
    {
        game.TakeTurn(player1 ? 0 : 1, 0);
        imgPos0.Source = new BitmapImage(new Uri(player1 ? "Assets\\Ladybug.png" : "Assets\\Bee.png", UriKind.Relative));

        btnPos0.IsEnabled = false;
        UpdatePlayer();
        GameOver(); 
    }

    private void btnPos1_Click(object sender, RoutedEventArgs e)
    {
        game.TakeTurn(player1 ? 0 : 1, 1); 
        imgPos1.Source = new BitmapImage(new Uri(player1 ? "Assets\\Ladybug.png" : "Assets\\Bee.png", UriKind.Relative));

        btnPos1.IsEnabled = false;
        UpdatePlayer();
        GameOver();
    }

    private void btnPos2_Click(object sender, RoutedEventArgs e)
    {
        game.TakeTurn(player1 ? 0 : 1, 2);
        imgPos2.Source = new BitmapImage(new Uri(player1 ? "Assets\\Ladybug.png" : "Assets\\Bee.png", UriKind.Relative));
        btnPos2.IsEnabled = false;
        UpdatePlayer();
        GameOver();
    }

    private void btnPos3_Click(object sender, RoutedEventArgs e)
    {
        game.TakeTurn(player1 ? 0 : 1, 3);
        imgPos3.Source = new BitmapImage(new Uri(player1 ? "Assets\\Ladybug.png" : "Assets\\Bee.png", UriKind.Relative));
        btnPos3.IsEnabled = false;
        UpdatePlayer();
        GameOver();
    }

    private void btnPos4_Click(object sender, RoutedEventArgs e)
    {
        game.TakeTurn(player1 ? 0 : 1, 4);
        imgPos4.Source = new BitmapImage(new Uri(player1 ? "Assets\\Ladybug.png" : "Assets\\Bee.png", UriKind.Relative));
        btnPos4.IsEnabled = false;
        UpdatePlayer();
        GameOver();
    }

    private void btnPos5_Click(object sender, RoutedEventArgs e)
    {
        game.TakeTurn(player1 ? 0 : 1, 5);
        imgPos5.Source = new BitmapImage(new Uri(player1 ? "Assets\\Ladybug.png" : "Assets\\Bee.png", UriKind.Relative));
        btnPos5.IsEnabled = false;
        UpdatePlayer();
        GameOver();
    }

    private void btnPos6_Click(object sender, RoutedEventArgs e)
    {
        game.TakeTurn(player1 ? 0 : 1, 6);
        imgPos6.Source = new BitmapImage(new Uri(player1 ? "Assets\\Ladybug.png" : "Assets\\Bee.png", UriKind.Relative));
        btnPos6.IsEnabled = false;
        UpdatePlayer();
        GameOver();
    }

    private void btnPos7_Click(object sender, RoutedEventArgs e)
    {
        game.TakeTurn(player1 ? 0 : 1, 7);
        imgPos7.Source = new BitmapImage(new Uri(player1 ? "Assets\\Ladybug.png" : "Assets\\Bee.png", UriKind.Relative));
        btnPos7.IsEnabled = false;
        UpdatePlayer();
        GameOver();
    }

    private void btnPos8_Click(object sender, RoutedEventArgs e)
    {
        game.TakeTurn(player1 ? 0 : 1, 8);
        imgPos8.Source = new BitmapImage(new Uri(player1 ? "Assets\\Ladybug.png" : "Assets\\Bee.png", UriKind.Relative));
        btnPos8.IsEnabled = false;
        UpdatePlayer();
        GameOver();
    }

    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        btnReset.Visibility = Visibility.Hidden;
        btnPos0.Visibility = Visibility.Visible;
        btnPos1.Visibility = Visibility.Visible;
        btnPos2.Visibility = Visibility.Visible;
        btnPos3.Visibility = Visibility.Visible;
        btnPos4.Visibility = Visibility.Visible;
        btnPos5.Visibility = Visibility.Visible;
        btnPos6.Visibility = Visibility.Visible;
        btnPos7.Visibility = Visibility.Visible;
        btnPos8.Visibility = Visibility.Visible;
        btnPos0.IsEnabled = true;
        btnPos1.IsEnabled = true;
        btnPos2.IsEnabled = true;
        btnPos3.IsEnabled = true;
        btnPos4.IsEnabled = true;
        btnPos5.IsEnabled = true;
        btnPos6.IsEnabled = true;
        btnPos7.IsEnabled = true;
        btnPos8.IsEnabled = true;
        btnPos0.Content = "";
        btnPos1.Content = "";
        btnPos2.Content = "";
        btnPos3.Content = "";
        btnPos4.Content = "";
        btnPos5.Content = "";
        btnPos6.Content = "";
        btnPos7.Content = "";
        btnPos8.Content = "";
        lblDisplay.Content = "Turn"; 
        imgIndicator.Source = new BitmapImage(new Uri("Assets\\Ladybug.png", UriKind.Relative));

        game = new();
        player1 = true;
    }
}