using System;
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
    private readonly int[] positions = [-1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1];

    /// <summary>
    /// Current state of the game board.
    /// Initialized to -1 in all positions.
    /// Empty = -1, 'X' = 0, 'O' = 1
    /// Read Only
    /// </summary>
    public int[] Positions => positions;
    /// <summary>
    /// Updates the current positions of the board according to what the player chooses.
    /// </summary>
    /// <param name="player">Player 1 = 0 = 'X', Player 2 = 1 = 'O'</param>
    /// <param name="position">Position is the 0-based index of the Positions array.</param>
    /// <returns>False if the position has already been selected, otherwise true.</returns>
    public bool TakeTurn(int player, int position)
    {
        if (player < 0 || player > 1)
            throw new ArgumentOutOfRangeException(nameof(player), player, "player can only be 0 or 1 representing player's 1 and 2 respectively.");
        if (position < 0 || position > 8)
            throw new ArgumentOutOfRangeException(nameof(position), position, "position can only be 0 thru 8 representing the 9 spaces on the board.");

        if (positions[position] != -1) return false; //players can only play on an empty tile. Trying to play on an occupied tile will fail
        positions[position] = player;
        return true;
    }
    /// <summary>
    /// Check every possible win condition.
    /// </summary>
    /// <returns>-1 if no win was found, 0 if Player 1 'X' won, 1 if player 2 'O' won. 2 if stalemate</returns>
    public WinResult CheckWin()
    {
        foreach (var condition in winConditions) //Check for a win
        {
            if (positions[condition[0]] == positions[condition[1]] && positions[condition[0]] == positions[condition[2]] && positions[condition[0]] != -1) // if a = b and a = c then b = c. Doing it this way avoids a nested loop maintaining constant time.
                return new WinResult(positions[condition[0]] == 0 ? WinType.Player1 : WinType.Player2, condition); //if a win is detected, then any/all of the tiles in the win condition contain the winning player
        }

        bool stalemate = true;
        foreach (var position in positions)          // Check for stalemate or incomplete game. This only runs if no outright win is detected. 
            stalemate = position != -1 && stalemate; // Stalemate only changes state if it is still true and the board contains an unplayed tile

        return new WinResult(stalemate ? WinType.Stalemate : WinType.None, positions);
    }
}
