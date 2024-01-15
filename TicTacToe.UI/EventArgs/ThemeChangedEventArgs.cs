using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.UI.EventArgs
{
    public class ThemeChangedEventArgs(ThemeManager theme)
    {
        public ThemeManager NewTheme { get; } = theme;
    }
}
