using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace TicTacToe.UI;
/// <summary>
/// Interaction logic for About.xaml
/// </summary>
public partial class About : Window
{
    public About()
    {
        InitializeComponent(); 
        SetUIVersion();
        SetAIVersion();
        SetCoreVersion();
        
    }

    private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

    private void SetUIVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        lblUIVersion.Content = versionInfo.FileVersion;

    }
    private void SetAIVersion()
    {
        var assembly = AI.Helpers.GetAssembly();
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        lblAIVersion.Content = versionInfo.FileVersion;

    }
    private void SetCoreVersion()
    {
        var assembly = Core.Helpers.GetAssembly();
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        lblCoreVersion.Content = versionInfo.FileVersion;

    }
}
