using System;
using System.Drawing;
using TicTacToe.Core;
using Xunit;
using static TicTacToe.Core.Tile;
using static TicTacToe.Core.WinResult;

namespace TicTacToe.Testing
{
    public class UnitTesting
    {
        private Board _sut;


        [Theory]
        [InlineData(new TileOwner[9] { TileOwner.Player1, TileOwner.Player1, TileOwner.Player1,
                                 TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                 TileOwner.Player2, TileOwner.Player2, TileOwner.Unclaimed }, WinType.Player1)] //Player 1 win - Row
        [InlineData(new TileOwner[9] { TileOwner.Player2, TileOwner.Player2, TileOwner.Player2,
                                 TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                 TileOwner.Player1, TileOwner.Player1, TileOwner.Unclaimed }, WinType.Player2)] //Player 2 win - Row
        [InlineData(new TileOwner[9] { TileOwner.Player2, TileOwner.Player1, TileOwner.Player1,
                                 TileOwner.Player2, TileOwner.Player1, TileOwner.Player1,
                                 TileOwner.Player2, TileOwner.Player2, TileOwner.Unclaimed }, WinType.Player2)] //Player 2 win - Column
        [InlineData(new TileOwner[9] { TileOwner.Player2, TileOwner.Player1, TileOwner.Player2,
                                 TileOwner.Player2, TileOwner.Player1, TileOwner.Player2,
                                 TileOwner.Player1, TileOwner.Player1, TileOwner.Unclaimed }, WinType.Player1)] //Player 1 win - Column
        [InlineData(new TileOwner[9] { TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                 TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                 TileOwner.Player2, TileOwner.Player1, TileOwner.Unclaimed }, WinType.Player2)] //Player 2 win - Diagonal
        [InlineData(new TileOwner[9] { TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                 TileOwner.Player2, TileOwner.Player1, TileOwner.Player2,
                                 TileOwner.Player1, TileOwner.Player1, TileOwner.Unclaimed }, WinType.Player1)] //Player 1 win - Diagonal
        [InlineData(new TileOwner[9] { TileOwner.Player2, TileOwner.Player1, TileOwner.Player2,
                                 TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                 TileOwner.Player1, TileOwner.Player2, TileOwner.Player1 }, WinType.Stalemate)] //Player 1 win - Stalemate
        [InlineData(new TileOwner[9] { TileOwner.Player2, TileOwner.Player1, TileOwner.Player2,
                                 TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                 TileOwner.Player1, TileOwner.Player2, TileOwner.Unclaimed }, WinType.None)] //Player 1 win - Incomplete Game
        [InlineData(new TileOwner[16] { TileOwner.Player1, TileOwner.Player1, TileOwner.Player1, TileOwner.Player1, 
                                        TileOwner.Player1, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player1, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player2, TileOwner.Player2, TileOwner.Player2, TileOwner.Unclaimed }, WinType.Player1, 4)] //Player 1 win - Row
        [InlineData(new TileOwner[16] { TileOwner.Player2, TileOwner.Player2, TileOwner.Player2, TileOwner.Player2,
                                        TileOwner.Player2, TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                        TileOwner.Player2, TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                        TileOwner.Player1, TileOwner.Player1, TileOwner.Player1, TileOwner.Unclaimed }, WinType.Player2, 4)] //Player 2 win - Row
        [InlineData(new TileOwner[16] { TileOwner.Player2, TileOwner.Player1, TileOwner.Player1, TileOwner.Player1,
                                        TileOwner.Player2, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player2, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player2, TileOwner.Player1, TileOwner.Player1, TileOwner.Unclaimed }, WinType.Player2, 4)] //Player 2 win - Column
        [InlineData(new TileOwner[16] { TileOwner.Player1, TileOwner.Player2, TileOwner.Player2, TileOwner.Player2,
                                        TileOwner.Player1, TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                        TileOwner.Player1, TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                        TileOwner.Player1, TileOwner.Player2, TileOwner.Player2, TileOwner.Unclaimed }, WinType.Player1, 4)] //Player 1 win - Column
        [InlineData(new TileOwner[16] { TileOwner.Player1, TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                        TileOwner.Player2, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player2, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player2, TileOwner.Player1, TileOwner.Player1, TileOwner.Unclaimed }, WinType.Player2, 4)] //Player 2 win - Diagonal
        [InlineData(new TileOwner[16] { TileOwner.Player2, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player1, TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                        TileOwner.Player1, TileOwner.Player1, TileOwner.Player1, TileOwner.Player2,
                                        TileOwner.Player1, TileOwner.Player2, TileOwner.Player2, TileOwner.Unclaimed }, WinType.Player1, 4)] //Player 1 win - Diagonal
        [InlineData(new TileOwner[16] { TileOwner.Player1, TileOwner.Player1, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player1, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player2, TileOwner.Player2, TileOwner.Player1, TileOwner.Player2,
                                        TileOwner.Player1, TileOwner.Player2, TileOwner.Player1, TileOwner.Player1 }, WinType.Stalemate, 4)] //Player 1 win - Stalemate
        [InlineData(new TileOwner[16] { TileOwner.Player1, TileOwner.Player1, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player1, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player2, TileOwner.Player2, TileOwner.Player2, TileOwner.Player1,
                                        TileOwner.Player1, TileOwner.Player2, TileOwner.Player1, TileOwner.Unclaimed }, WinType.None, 4)] //Player 1 win - Incomplete Game

        public void Play(TileOwner[] owner, WinType expected, int size = 3)
        {
            _sut = new Board(size);
            for (int i = 0; i < owner.Length; i++)
            {
                if (owner[i] is not TileOwner.Unclaimed)
                _sut.TakeTurn(owner[i], new(
                    (int)Math.Floor(i/(float)size), 
                    i % size));
            }
            var actual = _sut.CheckForWin(true);
            Assert.Equal(expected, actual.Winner);

        }
    }
}
