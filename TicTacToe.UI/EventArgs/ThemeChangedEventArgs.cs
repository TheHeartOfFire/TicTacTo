namespace TicTacToe.UI.EventArgs
{
    internal class ThemeChangedEventArgs(ThemeManager theme)
    {
        internal ThemeManager NewTheme { get; } = theme;
    }
}
