using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacTo.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Describes the various win conditions for end game lookup
        /// Each sub array contains the indicies for a row, column, or diagonal on a 3 x 3 board.
        /// </summary>
        private int[][] winConditions = new int[8][]
        {
            new int[3] {0, 1, 2 },
            new int[3] {3, 4, 5 },
            new int[3] {6, 7, 8 },
            new int[3] {0, 3, 6 },
            new int[3] {1, 4, 7 },
            new int[3] {2, 5, 8 },
            new int[3] {0, 4, 8 },
            new int[3] {2, 4, 6 }
        };
        /// <summary>
        /// Current state of the game board.
        /// Initialized to -1 in all positions.
        /// Empty = -1, 'X' = 0, 'O' = 1
        /// Read Only
        /// </summary>
        public int[] Positions { get; } = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };

        /// <summary>
        /// Updates the current positions of the board according to what the player chooses.
        /// </summary>
        /// <param name="player">Player 1 = 0 = 'X', Player 2 = 1 = 'O'</param>
        /// <param name="position">Position is the 0-based index of the Positions array.</param>
        /// <returns>False if the position has already been selected, otherwise true.</returns>
        public bool TakeTurn(int player, int position)
        {
            if (Positions[position] != -1) return false;
            Positions[position] = player;
            return true;
        }
        /// <summary>
        /// Check every possible win condition.
        /// </summary>
        /// <returns>-1 if no win was found, 0 if Player 1 'X' won, 1 if player 2 'O' won. 2 if stalemate</returns>
        public int CheckWin()
        {
            foreach(var condition in winConditions)
            {
                if (Positions[condition[0]] == Positions[condition[1]] && Positions[condition[0]] == Positions[condition[2]])
                    return Positions[condition[0]];
            }

            bool stalemate = true;
            foreach (var position in Positions)
                stalemate = position != -1 && stalemate;

            return stalemate? 2: -1;
        }
    }
}
