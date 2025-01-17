﻿using TicTacToe.AI.EventArgs;
using TicTacToe.AI.Interfaces;
using TicTacToe.Core;

namespace TicTacToe.AI;
public class BotManager
{
    //Using the singleton pattern to effectively have events in a static class
    public static BotManager Instance
    {
        get
        {
            _instance ??= new BotManager();
            return _instance;
        }
    }
    private static BotManager? _instance;

    public delegate void TurnOverEventHandler(object sender, TurnOverEventArgs e);
    public delegate void Notify();
    public event Notify? BotChanged;
    public ITicTacToeBot? Bot { get { return _bot; } set { _bot = value; OnBotChanged(); } }
    private ITicTacToeBot? _bot;
    protected virtual void OnBotChanged() => BotChanged?.Invoke();
    /// <summary>
    /// Ask the current bot to take it's turn
    /// </summary>
    /// <param name="game">The game that the bot needs to play on</param>
    /// <param name="turnTaken">The tile that the player claimed most recently.</param>
    /// <returns>The state of the board after the bot has taken it's turn</returns>
    /// <exception cref="InvalidOperationException">Bot cannot be null</exception>
    public Board TakeTurn(Board game, Tile? turnTaken)
    {
        if (_bot is null) throw new InvalidOperationException("Bot cannot be null!");
        return _bot.TakeTurn(game, turnTaken);
    
    }
}
