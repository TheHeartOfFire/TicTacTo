using System;
using System.Linq;
using TicTacToe.Core;
using static TicTacToe.Core.Tile;
using static TicTacToe.Core.WinResult;

namespace TicTacToe.Core;

/// <summary>
/// This class will instantiate a board, update the board according to player selections, and check the board to see if there is a win/tie.
/// </summary>
public class Board
{
    /// <summary>
    /// Current state of the game board.
    /// Read Only
    /// </summary>
    private readonly Tile[] positions;

    
    public Tile[] Positions => positions;

    public bool IsImminentStalemate = false;

    private readonly int boardSize;

    private Tile lastTilePlayed;

    public Board(int size = 3)
    {
        boardSize = size;
        //initialize tiles
        positions = new Tile[boardSize * boardSize];
        for (int i = 0; i < positions.Length; i++)
            positions[i] = new(i);
        //initialize win conditions
        WinCondition.FindWinConditions(boardSize);
    }
   



    /// <summary>
    /// Updates the current positions of the board according to what the player chooses.
    /// </summary>
    /// <param name="player">TileOwner.Player1 or TileOwner.Player2. TileOwner.Unclaimed will throw an error.</param>
    /// <param name="position">Position is the 0-based index of the Positions array.</param>
    /// <returns>False if the position has already been selected, otherwise true.</returns>
    public bool TakeTurn(TileOwner player, int position)
    {
        if (player is TileOwner.Unclaimed)
            throw new ArgumentException("You cannot unclaim a tile. Player must be either TileOwner.Player1 or TileOwner.Player2", nameof(player));
        if (position < 0 || position > (boardSize * boardSize) - 1)
            throw new ArgumentOutOfRangeException(nameof(position), position, "position can only be 0 thru (boardSize * 2) - 1 representing the valid indicies on the board.");

        if (positions[position].Owner is not TileOwner.Unclaimed) return false; //players can only play on an empty tile. Trying to play on an occupied tile will fail

        lastTilePlayed = positions[position];

        positions[position].Claim(player);
        return true;
    }
    /// <summary>
    /// Check the board against the list of win conditions and mark any tiles involved in a win as winning tiles.
    /// </summary>
    /// <returns>Returns a WinResult for the current state of the board.</returns>
    public WinResult CheckWin()
    {
        WinCondition.TrimImpossibleConditions(Positions);
        IsImminentStalemate = !WinCondition.IsWinPossible();

        //The shortest possible game is after player 1 has player boardSize number of times, during which Player 2 will have player boardSize - 1 times.
        //This means that the minimum game length is (boardSize * 2) -1
        if (positions.Where(pos => pos.Owner is not TileOwner.Unclaimed).Count() < (boardSize * 2) - 1) return new WinResult(Positions);

        foreach (var condition in WinCondition.WinConditions) //Check for a win
        {
            //Its not possible to win with a condition that does not include the last tile that was played
            if (!condition.Positions.Contains(lastTilePlayed.Index)) continue;

            //Ignore any conditions that aren't possible
            if(!condition.IsPossible) continue; 

            bool isWin = true;
            var comparison = positions[condition.Positions[0]];

            //ignore any conditions with unclaimed tiles
            if (comparison.Owner is TileOwner.Unclaimed) continue;

            foreach (var i in condition.Positions)
                isWin = comparison == positions[i] && isWin;

            if (isWin) MarkWinningTiles(condition);

        }

        return new WinResult(positions);
    }

    private void MarkWinningTiles(WinCondition condition)
    {
        foreach(var idx in condition.Positions)
            positions[idx].MarkAsWinner();
    }
}
