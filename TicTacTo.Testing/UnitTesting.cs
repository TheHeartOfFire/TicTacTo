using System.Security.Principal;
using TicTacTo.Core;
using Xunit;
using static TicTacTo.Core.WinResult;

namespace TicTacTo.Testing
{
    public class UnitTesting
    {
        private readonly Board _sut;

        public UnitTesting()
        {
            _sut = new Board();
        }

        [Theory]
        [InlineData(new int[9] { 0, 0, 0, 
                                 1, 1, 0, 
                                 1, 1, -1 }, WinType.PLAYER1)] //Player 1 win - Row
        [InlineData(new int[9] { 1, 1, 1, 
                                 0, 0, 1, 
                                 0, 0, -1 }, WinType.PLAYER2)] //Player 2 win - Row
        [InlineData(new int[9] { 1, 0, 0,
                                 1, 0, 0,
                                 1, 1, -1 }, WinType.PLAYER2)] //Player 2 win - Column
        [InlineData(new int[9] { 1, 0, 1,
                                 1, 0, 1,
                                 0, 0, -1 }, WinType.PLAYER1)] //Player 1 win - Column
        [InlineData(new int[9] { 0, 0, 1,
                                 1, 1, 0,
                                 1, 0, -1 }, WinType.PLAYER2)] //Player 2 win - Diagonal
        [InlineData(new int[9] { 1, 1, 0,
                                 1, 0, 1,
                                 0, 0, -1 }, WinType.PLAYER1)] //Player 1 win - Diagonal
        [InlineData(new int[9] { 1, 0, 1, 
                                 0, 0, 1, 
                                 0, 1, 0 }, WinType.STALEMATE)] //Player 1 win - Stalemate
        [InlineData(new int[9] { 1, 0, 1,
                                 0, 0, 1,
                                 0, 1, -1 }, WinType.NONE)] //Player 1 win - Incomplete Game
        public void Play(int[] positions, WinType expected)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                _sut.TakeTurn(positions[i], i);
            }
            var actual = _sut.CheckWin();
            Assert.Equal(expected, actual.Winner);

        }
    }
}
