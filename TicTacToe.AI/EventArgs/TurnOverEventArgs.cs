using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core;

namespace TicTacToe.AI.EventArgs;
public class TurnOverEventArgs(Tile turnTaken)
{
    public Tile TurnTaken => turnTaken;
}
