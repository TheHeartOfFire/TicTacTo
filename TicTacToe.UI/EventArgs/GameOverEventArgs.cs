using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core;

namespace TicTacToe.UI.EventArgs
{
    public class GameOverEventArgs (WinResult result)
    {
        public WinResult Result { get { return result; } }
    }
}
