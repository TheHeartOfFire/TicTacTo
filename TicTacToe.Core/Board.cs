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
    /// Each sub array contains the indicies for a row, column, or diagonal on a n x n board.
    /// </summary>
    private readonly int[][] winConditions;

    /// <summary>
    /// Current state of the game board.
    /// Read Only
    /// </summary>
    private readonly Tile[] positions;

    
    public Tile[] Positions => positions;

    private readonly int boardSize;

    public Board(int size = 3)
    {
        boardSize = size;
        //initialize tiles
        positions = new Tile[boardSize * boardSize];
        for (int i = 0; i < positions.Length; i++)
            positions[i] = new(i);
        //initialize win conditions
        winConditions = FindWinConditions(boardSize);

    }
    /// <summary>
    /// Generate win conditions for an n x n board
    /// </summary>
    /// <param name="size">The size of the board to find win conditions for</param>
    /// <returns>An int[][] of indicies for all rows, columns and diagonals to be used as win conditions</returns>
    private static int[][] FindWinConditions(int size)
    {
        //for a board of size n, there will be 2 diagonals, n cols, and n rows for a total of 2n+2 conditions
        var winConditions = new int[(2 * size) + 2][];

        int counter = 0;
        //Find rows

        // 0 | 1 | 2    0, 1, 2    i = row
        // 3 | 4 | 5 => 3, 4, 5 => j = col
        // 6 | 7 | 8    6, 7, 8    (i * size) + j
        for (int i = 0; i < size; i++)
        {
            winConditions[counter] = new int[size];   
            for (int j = 0; j < size; j++)
            {
                winConditions[counter][j] = (i * size) + j;
            }
            counter++;
        }

        //Find cols

        // 0 | 1 | 2    0, 3, 6    i = col
        // 3 | 4 | 5 => 1, 4, 7 => j = row
        // 6 | 7 | 8    2, 5, 8    i + (j * size)
        for (int i = 0; i < size; i++)
        {
            winConditions[counter] = new int[size];
            for (int j = 0; j < size; j++)
            {
                winConditions[counter][j] =  i + (j * size);
            }
            counter++;
        }

        //Find diagonals

        winConditions[counter] = new int[size];
        winConditions[counter + 1] = new int[size];
        // 0 | 1 | 2    0, 4, 8    (i * size) + i
        // 3 | 4 | 5 =>         => 
        // 6 | 7 | 8    6, 4, 2    (size * i) + (size - 1 - i)
        for (int i = 0; i < size; i++)
        {
            winConditions[counter][i] = (i * size) + i;//back diagonal
            winConditions[counter + 1][i] = (size * i) + (size - 1 - i);//front diagonal
        }

        return winConditions;
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
            bool isWin = true;
            var comparison = positions[condition[0]];

            if (comparison.Owner is TileOwner.Unclaimed) continue;

            foreach (var i in condition)
                isWin = comparison == positions[i] && isWin;

            if (isWin) MarkWinningTiles(condition);
                
        }

        return new WinResult(positions);
    }

    private void MarkWinningTiles(int[] indicies)
    {
        foreach(var idx in indicies)
            positions[idx].MarkAsWinner();
    }
}
