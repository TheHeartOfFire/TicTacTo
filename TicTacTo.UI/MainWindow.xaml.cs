using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TicTacTo.Core;
using static TicTacTo.UI.ThemeManager;

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
    private readonly Image[] verticalDividers;
    private readonly Image[] horizontalDividers;
    private ThemeManager theme;

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
        verticalDividers = [imgVerticalDivider0, imgVerticalDivider1, imgVerticalDivider2, imgVerticalDivider3];
        horizontalDividers = [imgHorizontalDivider0, imgHorizontalDivider1, imgHorizontalDivider2, imgHorizontalDivider3];
        ChangeTheme(Theme.BUG);
    }

    private void GameOver()
    {
        var winner = game.CheckWin();
        if (winner.Winner is WinResult.WinType.NONE) return;

        Cursor = Cursors.Arrow;

        lblDisplay.Content = "Winner!";

        if (winner.Winner is WinResult.WinType.STALEMATE)
            lblDisplay.Content = "Stalemate!";

        DisplayWinner(winner);

        btnReset.Visibility = Visibility.Visible;
    }

    private void DisplayWinner(WinResult result)
    {
        foreach (var btn in buttons)
            btn.IsEnabled = false;

        for (int i = 0; i < images.Length; i++)
        {
            if (!result.WinningTiles.Contains(i) && result.Winner is not WinResult.WinType.STALEMATE)
                images[i].Visibility = Visibility.Hidden;

            if (result.Winner is WinResult.WinType.STALEMATE)
                images[i].Source = (ImageSource)theme.ResDict["Stalemate"];
        }
    }

    private void UpdatePlayer()
    {
        player1 = !player1;
        Cursor = player1 ? theme.Player1Cursor : theme.Player2Cursor;
    }

    private void TakeTurn(Image img, Button btn, int pos)
    {
        game.TakeTurn(player1 ? 0 : 1, pos);
        img.Source = player1 ? (ImageSource)theme.ResDict["Player1"] : (ImageSource)theme.ResDict["Player2"];

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
        foreach (var img in images)
        {
            img.Source = null;
            img.Visibility = Visibility.Visible;
        }

        lblDisplay.Content = "";
        Cursor = theme.Player1Cursor;

        game = new();
        player1 = true;
    }

    private void ChangeTheme(Theme themeType)
    {
        theme = GetTheme(themeType);
        Application.Current.Resources.Clear();
        Application.Current.Resources.MergedDictionaries.Add(theme.ResDict);

        for (int i = 0; i < game.Positions.Count(); i++)//update tiles
        {
            if (game.Positions[i] == 0)
                images[i].Source = (ImageSource)theme.ResDict["Player1"];
            if (game.Positions[i] == 1)
                images[i].Source = (ImageSource)theme.ResDict["Player2"];
            if (game.CheckWin().Winner is WinResult.WinType.STALEMATE)
                images[i].Source = (ImageSource)theme.ResDict["Stalemate"];
        }

        Cursor = player1 ? theme.Player1Cursor : theme.Player2Cursor;//update cursor

        bool useBorder = theme.CurrentTheme is Theme.BUG;

        horizontalDividers[0].Visibility = useBorder ? Visibility.Visible : Visibility.Hidden;
        horizontalDividers[3].Visibility = useBorder ? Visibility.Visible : Visibility.Hidden;
        verticalDividers[0].Visibility = useBorder ? Visibility.Visible : Visibility.Hidden;
        verticalDividers[3].Visibility = useBorder ? Visibility.Visible : Visibility.Hidden;

    }

    private void menuBugTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.BUG);
    private void menuCandyTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.CANDY);
    private void menuTraditionalTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.TRADITIONAL);
    private void menuSeaTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.SEA);
    private void menuCardTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.CARD);

    private void menuCoinToss_Click(object sender, RoutedEventArgs e)
    {
        var coinToss = new CoinToss(theme);
        coinToss.Show();
    }
}