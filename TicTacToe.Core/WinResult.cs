using System.Collections.Generic;
using System.Linq;
using static TicTacToe.Core.Tile;

namespace TicTacToe.Core;
public class WinResult(Tile[,] board, bool stalemate = false)
{
    public enum WinType
    {
        None,
        Player1,
        Player2,
        Stalemate
    }
    //Get the indicies for all winning tiles.
    public List<Coordinates> WinningTileIndicies 
    { 
        get
        {
            List<Coordinates> tiles = [];

            foreach (var tile in board)
                if (tile.WinningTile)
                    tiles.Add(tile.Coords);
            return tiles;
        } 
    }

    public WinType Winner
    {
        get
        {
            if (stalemate) return WinType.Stalemate;
            //check for an outright winner
            foreach (var tile in board)
                if (tile.WinningTile)
                    return ConvertTileOwnerToWinType(tile.Owner);
            //check for an incomplete game
            if ((from tile in board.Cast<Tile>()
                 where tile.Owner is TileOwner.Unclaimed
                 select tile).Any()) return WinType.None;

            return WinType.Stalemate;
        }
    }

    public List<Tile> WinningTiles 
    { 
        get 
        {
            return (from tile in board.Cast<Tile>()
                    where tile.WinningTile
                    select tile).ToList(); 
        } 
    }

    private static WinType ConvertTileOwnerToWinType(TileOwner owner) => owner switch
    {
        TileOwner.Player1 => WinType.Player1,
        TileOwner.Player2 => WinType.Player2,
        _ => WinType.None,
    };
}
