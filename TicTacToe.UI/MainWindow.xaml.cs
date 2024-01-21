using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Squirrel;
using TicTacToe.AI;
using TicTacToe.AI.Interfaces;
using TicTacToe.UI.Controls;
using static TicTacToe.Core.Tile;
using static TicTacToe.UI.ThemeManager;

namespace TicTacToe.UI;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const string repoUrl = "https://github.com/TheHeartOfFire/TicTacToe";

    public MainWindow()
    {
        InitializeComponent();
        
        CheckForUpdates().ConfigureAwait(false);

        var game = new TicTacToeDisplay();
        grdGame.Children.Add(game);
    }
    //Menu Items

    //Themes
    private void menuBugTheme_Click(object sender, RoutedEventArgs e) => SetTheme(Theme.BUG);
    private void menuCandyTheme_Click(object sender, RoutedEventArgs e) => SetTheme(Theme.CANDY);
    private void menuTraditionalTheme_Click(object sender, RoutedEventArgs e) => SetTheme(Theme.TRADITIONAL);
    private void menuSeaTheme_Click(object sender, RoutedEventArgs e) => SetTheme(Theme.SEA);
    private void menuCardTheme_Click(object sender, RoutedEventArgs e) => SetTheme(Theme.CARD);

    //Tools
    private void menuCoinToss_Click(object sender, RoutedEventArgs e) => new CoinToss().Show();

    private void menuInstructions_Click(object sender, RoutedEventArgs e) => new Instructions().Show();

    private async Task CheckForUpdates()
    {
        using var manager = await UpdateManager.GitHubUpdateManager(repoUrl);

        Title = "TicTacToe - (Checking for updates)";

        try
        {
            var updateInfo = await manager.CheckForUpdate();

            if (!updateInfo.ReleasesToApply.Any())
            {
                Title = "TicTacToe";
                return;
            }

            Title = "TicTacToe - (Updates found! Downloading...)";

            await manager.UpdateApp();

            Title = "TicTacToe - (Restart to install update)";
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

    } 


    private void menuSize3_Checked(object sender, RoutedEventArgs e) => ChangeBoardSize(menuSize3);

    private void menuSize4_Checked(object sender, RoutedEventArgs e) => ChangeBoardSize(menuSize4, 4);

    private void menuSize5_Checked(object sender, RoutedEventArgs e) => ChangeBoardSize(menuSize5, 5);

    private void menuSize6_Checked(object sender, RoutedEventArgs e) => ChangeBoardSize(menuSize6, 6);

    private void ChangeBoardSize(MenuItem selectedItem, int size = 3)
    {
        if (grdGame is null) return;//omit call from initializing menuSize3 being defaulted to isChecked = true;

        foreach (MenuItem item in menuSize.Items)
            if (!item.Name.Equals(selectedItem.Name))
                item.IsChecked = false;


        var game = grdGame.Children[0] as TicTacToeDisplay;
        game?.Reset(size);
    }

    private void menuAbout_Click(object sender, RoutedEventArgs e) => new About().Show();

    private void menuNoBot_Checked(object sender, RoutedEventArgs e) => SetBot(menuNoBot, null);

    private void menuBadBot_Checked(object sender, RoutedEventArgs e) => SetBot(menuBadBot, new BadBot(menuBotOrderSecond.IsChecked ? TileOwner.Player2 : TileOwner.Player1));

    private void SetBot(MenuItem selectedItem, ITicTacToeBot? bot)
    {
        if (grdGame is null) return;//omit call from initializing menuNoBot being defaulted to isChecked = true;

        foreach (MenuItem item in menuBots.Items)
            if(!item.Name.Equals(selectedItem.Name)) 
                item.IsChecked = false;
        
        BotManager.Instance.Bot = bot;

    }
    private void menuBotOrderFirst_Checked(object sender, RoutedEventArgs e) => SetBotOrder(menuBotOrderFirst, TileOwner.Player1);

    private void menuBotOrderSecond_Checked(object sender, RoutedEventArgs e) => SetBotOrder(menuBotOrderSecond, TileOwner.Player2);

    private void SetBotOrder(MenuItem selectedItem, TileOwner order)
    {
        if (grdGame is null) return;//omit call from initializing menuBotOrderSecond being defaulted to isChecked = true;
        foreach (MenuItem item in menuBotOrder.Items)
            if (!item.Name.Equals(selectedItem.Name))
                item.IsChecked = false;
        if (BotManager.Instance.Bot is null) return; //don't bother trying to set the order for a bot that doesn't exist


        BotManager.Instance.Bot = BotManager.Instance.Bot?.New(order);
    }
}