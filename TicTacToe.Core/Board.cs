using System;
using System.Linq;
using static TicTacToe.Core.Tile;

namespace TicTacToe.Core;

/// <summary>
/// This class will instantiate a board, update the board according to player selections, and check the board to see if there is a win/tie.
/// </summary>
public class Board
{
    /// <summary>
    /// Current state of the game board.
    /// </summary>
    public Tile[,] Positions => positions;
    /// <summary>
    /// True if a stalemate is unavoidable due to the current state of the board
    /// </summary>
    public bool IsImminentStalemate { get; set; }
    /// <summary>
    /// The 1-indexed size of the board
    /// </summary>
    public int BoardSize => boardSize;
    /// <summary>
    /// True if the board is an even, False otherwise
    /// </summary>
    public bool IsEven { get { return boardSize % 2 == 0; } }
    /// <summary>
    /// The 0-indexed size of the board
    /// </summary>
    public int AdjustedBoardSize { get { return boardSize - 1; } }
    /// <summary>
    /// The center of the board
    /// </summary>
    public double Center { get { return boardSize / 2; } }
    private readonly Tile[,] positions;
    private readonly int boardSize;
    private Tile lastTilePlayed;

    public Board(int size = 3)
    {
        boardSize = size;
        //initialize tiles
        positions = new Tile[boardSize, boardSize];
        for (int i = 0; i < size; i++)
            for(int j = 0; j < size; j++)
                positions[i, j] = new(new(i, j));
        //initialize win conditions
        WinCondition.FindWinConditions(boardSize);
    }


    /// <summary>
    /// Updates the current positions of the board according to what the player chooses.
    /// </summary>
    /// <param name="player">TileOwner.Player1 or TileOwner.Player2. TileOwner.Unclaimed will throw an error.</param>
    /// <param name="position">Position is the 0-based index of the Positions array.</param>
    /// <returns>False if the position has already been selected, otherwise true</returns>
    /// <exception cref="ArgumentException">You cannot unclaim a tile. Player must be either TileOwner.Player1 or TileOwner.Player2</exception>
    /// <exception cref="ArgumentOutOfRangeException">position can only be 0 thru (boardSize * 2) - 1 representing the valid indicies on the board.</exception>
    public bool TakeTurn(TileOwner player, Coordinates position)
    {
        if (player is TileOwner.Unclaimed)
            throw new ArgumentException("You cannot unclaim a tile. Player must be either TileOwner.Player1 or TileOwner.Player2", nameof(player));
        if (position.X < 0 || position.X > boardSize - 1)
            throw new ArgumentOutOfRangeException(nameof(position), position.X, "Coordinates must be within the bounds of the array.");
        if (position.Y < 0 || position.Y > boardSize - 1)
            throw new ArgumentOutOfRangeException(nameof(position), position.Y, "Coordinates must be within the bounds of the array.");

        if (positions[position.X, position.Y].Owner is not TileOwner.Unclaimed) return false; //players can only play on an empty tile. Trying to play on an occupied tile will fail

        lastTilePlayed = positions[position.X, position.Y];

        positions[position.X, position.Y].Claim(player);
        return true;
    }
    /// <summary>
    /// Check the board against the list of win conditions and mark any tiles involved in a win as winning tiles.
    /// </summary>
    /// <returns>Returns a WinResult for the current state of the board.</returns>
    public WinResult CheckForWin(bool testing = false)
    {
        WinCondition.TrimImpossibleConditions(Positions);
        IsImminentStalemate = !WinCondition.IsWinPossible();

        //The shortest possible game is after player 1 has player boardSize number of times, during which Player 2 will have player boardSize - 1 times.
        //This means that the minimum game length is (boardSize * 2) -1
        var claimedTiles = positions.Cast<Tile>().Where(tile => tile.Owner is not TileOwner.Unclaimed);

        foreach (var condition in WinCondition.WinConditions) //Check for a win
        {
            if (IsImminentStalemate ||
                claimedTiles.Count() < (boardSize * 2) - 1) break;

            //Its not possible to win with a condition that does not include the last tile that was played
            if (!condition.Positions.Contains(lastTilePlayed.Coords) && !testing) continue;

            //Ignore any conditions that aren't possible
            if(!condition.IsPossible) continue; 

            bool isWin = true;
            var comparison = positions[condition.Positions[0].X, condition.Positions[0].Y];

            //ignore any conditions with unclaimed tiles
            if (comparison.Owner is TileOwner.Unclaimed) continue;
            
            foreach (var coords in condition.Positions)
                isWin = comparison == positions[coords.X, coords.Y] && isWin;

            if (isWin) MarkWinningTiles(condition);

        }

        var result = new WinResult(Positions, IsImminentStalemate);
        if (!positions.Cast<Tile>().Where(tile => tile.Owner is TileOwner.Unclaimed).Any())
            MarkWinningTiles(result);
        return result;
    }

    private void MarkWinningTiles(WinResult result)
    {
        if (result.Winner is WinResult.WinType.Stalemate)
            foreach (var tile in positions)
                tile.MarkAsWinner();
    }

    private void MarkWinningTiles(WinCondition condition)
    {

        foreach(var idx in condition.Positions)
            positions[idx.X, idx.Y].MarkAsWinner();
    }
}
