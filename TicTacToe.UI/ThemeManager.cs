using System.Windows;
using System.Windows.Input;

namespace TicTacToe.UI
{
    public class ThemeManager(Cursor player1Cursor, Cursor player2Cursor, ResourceDictionary resDict, ThemeManager.Theme theme)
    {
        private static readonly ThemeManager[] Themes = [
        new ThemeManager(
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Bug/Player1.cur", UriKind.Relative)).Stream),
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Bug/Player2.cur", UriKind.Relative)).Stream),
            new ResourceDictionary()
            {
                Source = new Uri("Assets/Themes/Bug/Theme.xaml", UriKind.Relative)
            }, Theme.BUG),
            new ThemeManager(
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Candy/Player1.cur", UriKind.Relative)).Stream),
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Candy/Player2.cur", UriKind.Relative)).Stream),
            new ResourceDictionary()
            {
                Source = new Uri("Assets/Themes/Candy/Theme.xaml", UriKind.Relative)
            }, Theme.CANDY),
            new ThemeManager(
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Traditional/Player1.cur", UriKind.Relative)).Stream),
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Traditional/Player2.cur", UriKind.Relative)).Stream),
            new ResourceDictionary()
            {
                Source = new Uri("Assets/Themes/Traditional/Theme.xaml", UriKind.Relative)
            }, Theme.TRADITIONAL),
            new ThemeManager(
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Card/Player1.cur", UriKind.Relative)).Stream),
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Card/Player2.cur", UriKind.Relative)).Stream),
            new ResourceDictionary()
            {
                Source = new Uri("Assets/Themes/Card/Theme.xaml", UriKind.Relative)
            }, Theme.CARD),
            new ThemeManager(
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Sea/Player1.cur", UriKind.Relative)).Stream),
            new Cursor(Application.GetResourceStream(new Uri("Assets/Themes/Sea/Player2.cur", UriKind.Relative)).Stream),
            new ResourceDictionary()
            {
                Source = new Uri("Assets/Themes/Sea/Theme.xaml", UriKind.Relative)
            }, Theme.SEA)
        ];

        public Cursor Player1Cursor { get; } = player1Cursor;
        public Cursor Player2Cursor { get; } = player2Cursor;
        public ResourceDictionary ResDict { get; } = resDict;
        public Theme CurrentTheme { get; } = theme;

        public enum Theme
        {
            BUG,
            CANDY,
            TRADITIONAL,
            CARD,
            SEA
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
