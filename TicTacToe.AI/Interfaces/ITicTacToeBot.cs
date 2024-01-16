using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core;

namespace TicTacToe.AI.Interfaces;
public interface ITicTacToeBot
{
    public Board TakeTurn(Board game, Tile tilePlayed);
}
