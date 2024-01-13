using System.Windows;
using System.Windows.Input;

namespace TicTacToe.UI
{
    public class ThemeManager
    {
        private static readonly string themePath = "TicTacToeWPF;component/Assets/Themes/";
        private static readonly ThemeManager[] Themes = [
        new ThemeManager(Theme.BUG),
            new ThemeManager(Theme.CANDY),
            new ThemeManager(Theme.TRADITIONAL),
            new ThemeManager(Theme.CARD),
            new ThemeManager(Theme.SEA)
        ];
        //TODO: Public Static Theme CurrentTheme;
        public Cursor Player1Cursor { get; }
        public Cursor Player2Cursor { get; }
        public ResourceDictionary ResDict { get; }
        public Theme CurrentTheme { get; }

        public enum Theme
        {
            BUG,
            CANDY,
            TRADITIONAL,
            CARD,
            SEA
        }

        public ThemeManager(Theme theme)
        {
            CurrentTheme = theme;
            ResDict = new ResourceDictionary()
            {
                Source = new Uri(themePath + theme + "/Theme.xaml", UriKind.Relative)
            };
            Player1Cursor = new Cursor(Application.GetResourceStream(new Uri(themePath + theme + "/Player1.cur", UriKind.Relative)).Stream);
            Player2Cursor = new Cursor(Application.GetResourceStream(new Uri(themePath + theme + "/Player2.cur", UriKind.Relative)).Stream);
        }

        public static ThemeManager GetTheme(Theme theme)
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


    }
}
