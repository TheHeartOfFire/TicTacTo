using TicTacToe.Core;

namespace TicTacToe.AI.EventArgs;
public class TurnOverEventArgs(Tile turnTaken)
{
    public Tile TurnTaken => turnTaken;
}
