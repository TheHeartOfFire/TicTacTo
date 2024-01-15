using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core;
public class WinCondition(int[] positions)
{
    public int[] Positions { get; } = positions;
    public bool IsPossible { get; private set; } = true;
    public static WinCondition[] WinConditions { get; private set; }

    public void CheckIfPossible(Tile[] board)
    {
        //don't bother checking conditions that are already impossible
        if(!IsPossible) return;

        //Any win condition that contains a tile claimed by both players cannot be a valid win condition
        bool player1ClaimedATile = false;
        bool player2ClaimedATile = false;

        List<Tile> relevantTiles = [];

        foreach(int pos in Positions)
            relevantTiles.Add(board[pos]);

        player1ClaimedATile = relevantTiles.Where(tile => tile.Owner is Tile.TileOwner.Player1).Any();
        player2ClaimedATile = relevantTiles.Where(tile => tile.Owner is Tile.TileOwner.Player2).Any();
        IsPossible = !(player1ClaimedATile && player2ClaimedATile);
    }

    public static void TrimImpossibleConditions(Tile[] board)
    {
        foreach(var condition in WinConditions)
            condition.CheckIfPossible(board);
    }
    
    public static bool IsWinPossible()
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
        //Find rows

        // Board        Desired    Equation
        // 0 | 1 | 2    0, 1, 2    i = row
        // 3 | 4 | 5 => 3, 4, 5 => j = col
        // 6 | 7 | 8    6, 7, 8    (i * size) + j
        for (int i = 0; i < size; i++)
        {
            winConditions[counter] = new WinCondition(new int[size]);
            for (int j = 0; j < size; j++)
            {
                winConditions[counter].Positions[j] = (i * size) + j;
            }
            counter++;
        }

        //Find cols

        // Board        Desired    Equation
        // 0 | 1 | 2    0, 3, 6    i = col
        // 3 | 4 | 5 => 1, 4, 7 => j = row
        // 6 | 7 | 8    2, 5, 8    i + (j * size)
        for (int i = 0; i < size; i++)
        {
            winConditions[counter] = new WinCondition(new int[size]);
            for (int j = 0; j < size; j++)
            {
                winConditions[counter].Positions[j] = i + (j * size);
            }
            counter++;
        }

        //Find diagonals

        winConditions[counter] = new WinCondition(new int[size]);
        winConditions[counter + 1] = new WinCondition(new int[size]);

        // Board        Desired    Equation
        // 0 | 1 | 2    0, 4, 8    (i * size) + i
        // 3 | 4 | 5 =>         => 
        // 6 | 7 | 8    6, 4, 2    (size * i) + (size - 1 - i)
        for (int i = 0; i < size; i++)
        {
            winConditions[counter].Positions[i] = (i * size) + i;//back diagonal
            winConditions[counter + 1].Positions[i] = (size * i) + (size - 1 - i);//front diagonal
        }

        WinConditions = winConditions;
    }
}
