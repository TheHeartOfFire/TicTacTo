using System;
using static TicTacToe.Core.Tile;

namespace TicTacToe.Core;
public class Tile(Coordinates coords) : IEquatable<Tile>
{
    public enum TileOwner
    {
        Unclaimed,
        Player1,
        Player2
    }
    public struct Coordinates(int x, int y)
    {
        public int X = x;
        public int Y = y;
    }
    public TileOwner Owner { get{ return _owner; } }
    public Coordinates Coords { get; } = coords;
    public bool WinningTile { get { return _winningTile; } }

    private TileOwner _owner = TileOwner.Unclaimed;
    private bool _winningTile = false;


    public void Claim(TileOwner owner) => _owner = owner;
    public void MarkAsWinner() => _winningTile = true;

    public static bool operator ==(Tile firstTile, Tile secondTile)
    {
        if (firstTile is null)
        {
            if (secondTile is null) return true;

            // Only the left side is null.
            return false;
        }

        // Only the right side is null
        if (secondTile is null) return false;

        return firstTile.Owner == secondTile.Owner;
    }

    public static bool operator !=(Tile firstTile, Tile secondTile) => !(firstTile == secondTile);

    //IEquatable stuff
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        return ((Tile)obj)._owner == _owner;
    }

    public override int GetHashCode()
    {
        return _owner.GetHashCode();
    }

    public bool Equals(Tile other)
    {
        return _owner == other._owner;
    }
}
