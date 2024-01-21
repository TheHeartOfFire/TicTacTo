using TicTacToe.Core;

namespace TicTacToe.UI.EventArgs
{
    public class GameOverEventArgs (WinResult result)
    {
        public WinResult Result { get { return result; } }
    }
}
