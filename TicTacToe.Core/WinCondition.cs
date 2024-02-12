using System;
using System.Linq;
using static TicTacToe.Core.Tile;

namespace TicTacToe.Core;
public class WinCondition(Coordinates[] positions)
{
    public static WinCondition[] WinConditions { get; private set; }
    public Coordinates[] Positions { get { return positions; } }
    public bool IsPossible { get; private set; } = true;
    private readonly Coordinates[] positions = positions;

    /// <summary>
    /// Check each win condition against the current state of the board and mark impossible conditions appropriately.
    /// </summary>
    /// <param name="board">The current board state</param>
    internal static void TrimImpossibleConditions(Tile[,] board)
    {
        foreach(var condition in WinConditions)
            condition.CheckIfPossible(board);
    }
    /// <summary>
    /// Check to see if any of the win conditions are still possible
    /// </summary>
    /// <returns>True if at least 1 win condition is still possible.</returns>
    internal static bool IsWinPossible()
    {
        //if there are no more possible win conditions then a stalemate is unavoidable.
        foreach(var condition in WinConditions)
            if(condition.IsPossible) return true;
        return false;
    }

    /// <summary>
    /// Generate win conditions for an n x n board
    /// </summary>
    /// <param name="size">The size of the board to find win conditions for</param>
    /// <returns>An WinCondition[] of indicies for all rows, columns and diagonals to be used as win conditions</returns>
    internal static void FindWinConditions(int size)
    {
        //for a board of size n, there will be 2 diagonals, n cols, and n rows for a total of 2n+2 conditions
        var winConditions = new WinCondition[(2 * size) + 2];

        int counter = 0;

        //initialize diagonals
        winConditions[^1] = new WinCondition(new Coordinates[size]);
        winConditions[^2] = new WinCondition(new Coordinates[size]);
        // Board        Desired    Coords
        // 0 | 1 | 2    0, 1, 2    i = row                     |
        // 3 | 4 | 5 => 3, 4, 5 => j = col                     | Rows
        // 6 | 7 | 8    6, 7, 8    i, j                        |
        //-----------------------------------------------------|
        // 0 | 1 | 2    0, 3, 6    i = row                     |
        // 3 | 4 | 5 => 1, 4, 7 => j = col                     | Columns
        // 6 | 7 | 8    2, 5, 8    j, i                        |
        //-----------------------------------------------------|
        // 0 | 1 | 2    0, 4, 8    i, i                        | back diagonal
        // 3 | 4 | 5 =>         =>                             |
        // 6 | 7 | 8    6, 4, 2    i, (size - 1 - i)           | front diagonal
        for (int i = 0; i < size; i++)
        {
            winConditions[counter] = new WinCondition(new Coordinates[size]);
            winConditions[counter+1] = new WinCondition(new Coordinates[size]);
            for (int j = 0; j < size; j++)
            {
                winConditions[counter].Positions[j] = new(j,i);//row
                winConditions[counter+1].Positions[j] = new(i,j);//col
            }
            counter+=2;
            winConditions[^1].Positions[i] = new(i, i);//back diagonal
            winConditions[^2].Positions[i] = new(i , (size - 1 - i));//front diagonal
        }

        WinConditions = winConditions;
    }

    /// <summary>
    /// Check to see if this win condition isstill a viable option
    /// </summary>
    /// <param name="board">The board to check the win condition against.</param>
    internal void CheckIfPossible(Tile[,] board)
    {
        //don't bother checking conditions that are already impossible
        if(!IsPossible) return;

        var relevantTiles = from tile in board.Cast<Tile>()
                            where positions.Contains(tile.Coords)
                            select tile;//board.Where(tile => positions.Contains(tile.Index)).ToList();

        //Any win condition that contains a tile claimed by both players cannot be a valid win condition
        var player2ClaimedATile = relevantTiles.Where(tile => tile.Owner is Tile.TileOwner.Player2).Any();
        var player1ClaimedATile = relevantTiles.Where(tile => tile.Owner is Tile.TileOwner.Player1).Any();
        IsPossible = !(player1ClaimedATile && player2ClaimedATile);
    }
}
