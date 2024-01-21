using TicTacToe.Core;
using static TicTacToe.AI.BotManager;
using static TicTacToe.Core.Tile;

namespace TicTacToe.AI.Interfaces;
public interface ITicTacToeBot
{
    public TileOwner Order { get; set; }
    public event TurnOverEventHandler? TurnOver;
    public Board TakeTurn(Board game, Tile? tilePlayed);
    public ITicTacToeBot New(TileOwner order);
}
