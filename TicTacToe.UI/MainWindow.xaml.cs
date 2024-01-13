using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Squirrel;
using TicTacToe.Core;
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
    private ThemeManager Theme;
    private TicTacToeDisplay game;

    public MainWindow()
    {
        InitializeComponent();
        
        CheckForUpdates().ConfigureAwait(false);
        AddVersionNumber();


        game = new TicTacToeDisplay();
        Grid.SetRow(game, 1);
        grdGame.Children.Add(game);

        ChangeTheme(ThemeManager.Theme.BUG, out Theme);

    }
   
    /// <summary>
    /// Applies the specified theme to the window
    /// </summary>
    /// <param name="themeType"></param>
    private void ChangeTheme(Theme themeType)=>ChangeTheme(themeType, out Theme);

    private void ChangeTheme(Theme themeType, out ThemeManager theme)
    {
        theme = GetTheme(themeType);
        Application.Current.Resources.Clear();
        Application.Current.Resources.MergedDictionaries.Add(Theme.ResDict);//Change the active resource dictionary

        game.ChangeTheme(themeType);
    }
    //Menu Items

    //Themes
    private void menuBugTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(ThemeManager.Theme.BUG);
    private void menuCandyTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(ThemeManager.Theme.CANDY);
    private void menuTraditionalTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(ThemeManager.Theme.TRADITIONAL);
    private void menuSeaTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(ThemeManager.Theme.SEA);
    private void menuCardTheme_Click(object sender, RoutedEventArgs e) => ChangeTheme(ThemeManager.Theme.CARD);

    //Tools
    private void menuCoinToss_Click(object sender, RoutedEventArgs e) => Debug.WriteLine(e.OriginalSource);//new CoinToss(theme).Show();

    private void menuInstructions_Click(object sender, RoutedEventArgs e) => new Instructions().Show();

    private async Task CheckForUpdates()
    {
        using var manager = await UpdateManager.GitHubUpdateManager(repoUrl);

            await manager.UpdateApp();
    } 

    private void AddVersionNumber()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        Title = Title + " " + versionInfo.FileVersion;
    }
}