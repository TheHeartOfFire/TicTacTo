using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
