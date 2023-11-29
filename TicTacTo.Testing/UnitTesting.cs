using TicTacTo.Core;
using Xunit;

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
        [InlineData(new int[9] { 0, 0, 0, 1, 1, 0, 1, 1, -1 }, 0)]
        [InlineData(new int[9] { 1, 1, 1, 0, 0, 1, 0, 0, -1 }, 1)]
        [InlineData(new int[9] { 1, 0, 1, 0, 0, 1, 0, 1, 0 }, 2)]
        public void Play(int[] positions, int expected)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                _sut.TakeTurn(positions[i], i);
            }
            var actual = _sut.CheckWin();
            Assert.Equal(expected, actual);

        }
    }
}
