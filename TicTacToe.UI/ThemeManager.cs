using System.Windows;
using System.Windows.Input;

namespace TicTacToe.UI
{
    internal class ThemeManager
    {
        internal enum Theme
        {
            BUG,
            CANDY,
            TRADITIONAL,
            CARD,
            SEA
        }
        internal event EventHandler? ThemeChanged;
        internal Cursor Player1Cursor { get; }
        internal Cursor Player2Cursor { get; }
        internal ResourceDictionary ResDict { get; }
        internal Theme CurrentTheme { get; }
        protected virtual void OnThemeChanged() => ThemeChanged?.Invoke(this, System.EventArgs.Empty);

        private static readonly string themePath = "TicTacToeWPF;component/Assets/Themes/";
        private static readonly ThemeManager[] Themes =
            [
                new ThemeManager(Theme.BUG),
                new ThemeManager(Theme.CANDY),
                new ThemeManager(Theme.TRADITIONAL),
                new ThemeManager(Theme.CARD),
                new ThemeManager(Theme.SEA)
        ];

        internal static ThemeManager ActiveTheme { get; private set; } = Themes[0];
        internal static ThemeManager GetTheme(Theme theme)
        {
            return theme switch
            {
                Theme.BUG => Themes[0],
                Theme.CANDY => Themes[1],
                Theme.TRADITIONAL => Themes[2],
                Theme.CARD => Themes[3],
                Theme.SEA => Themes[4],
                _ => Themes[0],
            };
        }
        internal static void SetTheme(Theme theme) => SetTheme(GetTheme(theme));
        internal static void SetTheme(ThemeManager theme)
        {
            var oldTheme = ActiveTheme;

            ActiveTheme = theme; 
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme.ResDict);//Change the active resource dictionary

            oldTheme.OnThemeChanged();
        }
        internal ThemeManager(Theme theme)
        {
            CurrentTheme = theme;
            ResDict = new ResourceDictionary()
            {
                Source = new Uri(themePath + theme + "/Theme.xaml", UriKind.Relative)
            };
            Player1Cursor = new Cursor(Application.GetResourceStream(new Uri(themePath + theme + "/Player1.cur", UriKind.Relative)).Stream);
            Player2Cursor = new Cursor(Application.GetResourceStream(new Uri(themePath + theme + "/Player2.cur", UriKind.Relative)).Stream);
        }
    }
}
