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
        private readonly int[][] winConditions =
        [
            [0, 1, 2],
            [3, 4, 5],
            [6, 7, 8],
            [0, 3, 6],
            [1, 4, 7],
            [2, 5, 8],
            [0, 4, 8],
            [2, 4, 6]
        ];
        private readonly int[] positions = [-1, -1, -1, -1, -1, -1, -1, -1, -1];

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
            if (positions[position] != -1) return false; //players can only play on an empty tile. Trying to play on an occupied tile will fail
            positions[position] = player;
            return true;
        }
        /// <summary>
        /// Check every possible win condition.
        /// </summary>
        /// <returns>-1 if no win was found, 0 if Player 1 'X' won, 1 if player 2 'O' won. 2 if stalemate</returns>
        public int CheckWin()
        {
            foreach (var condition in winConditions) //Check for a win
            {
                if (positions[condition[0]] == positions[condition[1]] && positions[condition[0]] == positions[condition[2]]) // if a = b and a = c then b = c
                    return positions[condition[0]]; //if a win is detected, then any/all of the tiles in the win condition contain the winning player
            }

            bool stalemate = true;
            foreach (var position in positions)          // Check for stalemate or incomplete game. This only runs if no outright win is detected. 
                stalemate = position != -1 && stalemate; // Stalemate only changes state if it is still true and the board contains an unplayed tile

            return stalemate ? 2 : -1;
        }
    }
}
