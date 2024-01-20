using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI.EventArgs;
using TicTacToe.AI.Interfaces;
using TicTacToe.Core;
using static TicTacToe.AI.BotManager;
using static TicTacToe.Core.Tile;

namespace TicTacToe.AI;
public class BadBot : ITicTacToeBot
{
    private event TurnOverEventHandler? turnOver;
    public event TurnOverEventHandler? TurnOver
    {
        add
        {
            turnOver -= value;
            turnOver += value;
        }
        remove
        {
            turnOver -= value;
        }
    }
    protected virtual void OnTurnOver(TurnOverEventArgs e) => turnOver?.Invoke(this, e);

    public Board TakeTurn(Board game, Tile? tilePlayed) => PickTile(game);
    private TileOwner _order;
    public TileOwner Order { 
        get
        {
            return _order;
        } 
        set
        {
            if (value is TileOwner.Unclaimed) throw new ArgumentException("Order cannot be TileOwner.Unclaimed. Use TileOwner.Player1 or TileOwner.Player2");
            _order = value;
        }
    }


    public BadBot(TileOwner order)
    {
        if(order is TileOwner.Unclaimed) throw new ArgumentException("Order cannot be TileOwner.Unclaimed. Use TileOwner.Player1 or TileOwner.Player2");
        _order = order;
    }

    private Board PickTile(Board game)
    {
        List<Tile> unclaimedTiles = [];

        foreach(var tile in game.Positions)
            if(tile.Owner is TileOwner.Unclaimed) unclaimedTiles.Add(tile);

        if (unclaimedTiles.Count <= 0) return game;

        var rand = new Random();
        var index = rand.Next(0, unclaimedTiles.Count);
        var selectedTile = unclaimedTiles[index];
        game.TakeTurn(Order, selectedTile.Index);

        OnTurnOver(new TurnOverEventArgs(selectedTile));
        return game;

    }

    public ITicTacToeBot New(TileOwner order) => new BadBot(order);
}
