namespace TicTacToe.Core;
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
