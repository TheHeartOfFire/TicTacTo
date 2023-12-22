using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacTo.Core;
public class WinResult(WinResult.WinType winner, int[] winningTiles)
{
    public enum WinType
    {
        NONE,
        PLAYER1,
        PLAYER2,
        STALEMATE
    }

    public WinType Winner { get; } = winner;
    public int[] WinningTiles { get; } = winningTiles;
}
