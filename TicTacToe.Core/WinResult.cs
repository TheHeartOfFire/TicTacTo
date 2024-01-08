using System.Collections.Generic;
using System.Linq;
using static TicTacToe.Core.Tile;

namespace TicTacToe.Core;
public class WinResult(Tile[] board)
{
    public enum WinType
    {
        None,
        Player1,
        Player2,
        Stalemate
    }

    public WinType Winner
    {
        get
        {
            foreach (var tile in board)
                if (tile.WinningTile)
                    return ConvertOwnerToWinType(tile.Owner);

            if (board.Where(tile => tile.Owner is TileOwner.Unclaimed).Any()) return WinType.None;
            return WinType.Stalemate;
        }
    }

    public List<int> WinningTileIndicies { get
        {
            List<int> tiles = [];

            foreach (var tile in board)
                tiles.Add(tile.Index);
            return tiles;
        } }

    public List<Tile> WinningTiles { get { return board.Where(tile => tile.WinningTile).ToList(); } }

    private static WinType ConvertOwnerToWinType(TileOwner owner) => owner switch
    {
        TileOwner.Player1 => WinType.Player1,
        TileOwner.Player2 => WinType.Player2,
        _ => WinType.None,
    };
    
}
