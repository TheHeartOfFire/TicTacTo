namespace TicTacToe.Core;
public class WinResult(WinResult.WinType winner, int[] winningTiles)
{
    public enum WinType
    {
        None,
        Player1,
        Player2,
        Stalemate
    }

    public WinType Winner { get; } = winner;
    public int[] WinningTiles { get; } = winningTiles;
}
