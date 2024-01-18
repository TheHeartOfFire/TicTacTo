using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI.Interfaces;
using TicTacToe.Core;

namespace TicTacToe.AI;
public class BotManager
{
    private static BotManager? _instance;
    public static BotManager Instance
    {
        get
        {
            _instance ??= new BotManager();
            return _instance;
        }
    }
    private ITicTacToeBot? _bot;
    public ITicTacToeBot? Bot { get { return _bot; } set { _bot = value; OnBotChanged(); } }
    public delegate void Notify();
    public event Notify? BotChanged;
    protected virtual void OnBotChanged() => BotChanged?.Invoke();

    public Board TakeTurn(Board game, Tile? turnTaken)
    {
        if (_bot is null) throw new InvalidOperationException("Bot cannot be null!");

        return _bot.TakeTurn(game, turnTaken);
    }
}
