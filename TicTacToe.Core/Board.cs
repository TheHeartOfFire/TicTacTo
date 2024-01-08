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
    /// Describes the various win conditions for end game lookup
    /// Each sub array contains the indicies for a row, column, or diagonal on a 3 x 3 board.
    /// </summary>
    private readonly int[][] winConditions =
    [
        [0, 1, 2], //Row 1
        [3, 4, 5], //Row 2
        [6, 7, 8], //Row 3
        [0, 3, 6], //Column 1
        [1, 4, 7], //Column 2
        [2, 5, 8], //Column 3
        [0, 4, 8], //Back Diagonal
        [2, 4, 6]  //Front Diagonal
    ];
    /// <summary>
    /// Current state of the game board.
    /// Initialized to TileOwner.Unclaimed in all positions.
    /// Read Only
    /// </summary>
    private readonly Tile[] positions = [new(0),
        new(1),
        new(2),
        new(3),
        new(4),
        new(5),
        new(6),
        new(7),
        new(8)];

    
    public Tile[] Positions => positions;
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
        if (position < 0 || position > 8)
            throw new ArgumentOutOfRangeException(nameof(position), position, "position can only be 0 thru 8 representing the 9 spaces on the board.");

        if (positions[position].Owner is not TileOwner.Unclaimed) return false; //players can only play on an empty tile. Trying to play on an occupied tile will fail
        positions[position].Claim(player);
        return true;
    }
    /// <summary>
    /// Check the board against the list of win conditions and mark any tiles involved in a win as winning tiles.
    /// </summary>
    /// <returns>Returns a WinResult for the current state of the board.</returns>
    public WinResult CheckWin()
    {
        foreach (var condition in winConditions) //Check for a win
        {
            if (positions[condition[0]] == positions[condition[1]]
                && positions[condition[0]] == positions[condition[2]]
                && positions[condition[0]].Owner is not TileOwner.Unclaimed) // if a = b and a = c then b = c. 
            {
               MarkWinningTiles(condition);
            }
                
        }

        return new WinResult(positions);
    }

    private void MarkWinningTiles(int[] indicies)
    {
        foreach(var idx in indicies)
            positions[idx].MarkAsWinner();
    }
}
