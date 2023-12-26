using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TicTacToe.Core;
using static TicTacToe.UI.ThemeManager;

namespace TicTacToe.UI;
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
    /// <summary>
    /// Check for a win condition
    /// </summary>
    private void GameOver()
    {
        var winner = game.CheckWin();//Get the current WinStatus of the game
        if (winner.Winner is WinResult.WinType.None) return;//If the game is still in progress, do nothing

        Cursor = Cursors.Arrow;//Change the cursor back to the normal one as no players are taking a turn

        lblDisplay.Content = "Winner!";

        if (winner.Winner is WinResult.WinType.Stalemate)//If the game was decisive, Tell the player there wass a winner, otherwise, tell them it was a stalemate.
            lblDisplay.Content = "Stalemate!";

        DisplayWinner(winner);//Display the results of the game

        btnReset.Visibility = Visibility.Visible;//Allow the players to start a new game
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
            if (!result.WinningTiles.Contains(i) && result.Winner is not WinResult.WinType.Stalemate)
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
        game.TakeTurn(player1 ? 0 : 1, pos);//process the turn
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
    /// Start a new game
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        btnReset.Visibility = Visibility.Hidden;//Hide the reset button. This should only be visible at the end of the game.

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

        lblDisplay.Content = "";//This label is only visible at the end of the game.
        Cursor = theme.Player1Cursor;//reset the cursor

        game = new();//Create a new game board to play on
        player1 = true;
    }

    /// <summary>
    /// Applies the specified theme to the window
    /// </summary>
    /// <param name="themeType"></param>
    private void ChangeTheme(Theme themeType)
    {
        theme = GetTheme(themeType);
        Application.Current.Resources.Clear();
        Application.Current.Resources.MergedDictionaries.Add(theme.ResDict);//Change the active resource dictionary

        for (int i = 0; i < game.Positions.Count(); i++)//update tiles
        {
            if (game.Positions[i] == 0)
                images[i].Source = (ImageSource)theme.ResDict["Player1"];
            if (game.Positions[i] == 1)
                images[i].Source = (ImageSource)theme.ResDict["Player2"];
            if (game.CheckWin().Winner is WinResult.WinType.Stalemate)
                images[i].Source = (ImageSource)theme.ResDict["Stalemate"];
        }

        Cursor = player1 ? theme.Player1Cursor : theme.Player2Cursor;//update cursor

        bool useBorder = theme.CurrentTheme is Theme.BUG;//currently only the bug theme uses the outer borders. Future themes may also use them.

        horizontalDividers[0].Visibility = useBorder ? Visibility.Visible : Visibility.Hidden;
        horizontalDividers[3].Visibility = useBorder ? Visibility.Visible : Visibility.Hidden;
        verticalDividers[0].Visibility = useBorder ? Visibility.Visible : Visibility.Hidden;
        verticalDividers[3].Visibility = useBorder ? Visibility.Visible : Visibility.Hidden;

    }
    //Menu Items

    //Themes
    private void menuBugTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.BUG);
    private void menuCandyTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.CANDY);
    private void menuTraditionalTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.TRADITIONAL);
    private void menuSeaTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.SEA);
    private void menuCardTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(Theme.CARD);

    //Tools
    private void menuCoinToss_Click(object sender, RoutedEventArgs e) => new CoinToss(theme).Show();

    private void menuInstructions_Click(object sender, RoutedEventArgs e) => new Instructions().Show();
}