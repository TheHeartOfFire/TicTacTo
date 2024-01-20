using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI.EventArgs;
using TicTacToe.Core;
using static TicTacToe.AI.BotManager;
using static TicTacToe.Core.Tile;

namespace TicTacToe.AI.Interfaces;
public interface ITicTacToeBot
{
    public Board TakeTurn(Board game, Tile? tilePlayed);
    public event TurnOverEventHandler? TurnOver;
    public TileOwner Order { get; set; }
    public ITicTacToeBot New(TileOwner order);
}
