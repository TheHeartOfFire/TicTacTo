using TicTacToe.AI.EventArgs;
using TicTacToe.AI.Interfaces;
using TicTacToe.Core;
using static TicTacToe.AI.BotManager;
using static TicTacToe.Core.Tile;

namespace TicTacToe.AI;
/// <summary>
/// This bot will choose it's turn by randomly picking any available tile.
/// </summary>
public class BadBot : ITicTacToeBot
{
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
    private event TurnOverEventHandler? turnOver;
    protected virtual void OnTurnOver(TurnOverEventArgs e) => turnOver?.Invoke(this, e);
    private TileOwner _order;
    private Random _random;

    public Board TakeTurn(Board game, Tile? tilePlayed) => PickTile(game);
    public ITicTacToeBot New(TileOwner order) => new BadBot(order);


    public BadBot(TileOwner order)
    {
        if(order is TileOwner.Unclaimed) throw new ArgumentException("Order cannot be TileOwner.Unclaimed. Use TileOwner.Player1 or TileOwner.Player2");
        _order = order;
        _random = new Random();
    }
    /// <summary>
    /// Chooses a random unclaimed tile and plays it as the bot's turn.
    /// </summary>
    /// <param name="game">The board that the bot should take a turn on.</param>
    /// <returns>The board after the bot has taken it's turn</returns>
    private Board PickTile(Board game)
    {
        //get all of the unclaimed tiles
        var unclaimedTiles = game.Positions.Where(tile => tile.Owner is TileOwner.Unclaimed).ToList();
        if (unclaimedTiles.Count <= 0) return game;
        //pick one at random
        var selectedTile = unclaimedTiles[_random.Next(0, unclaimedTiles.Count())];

        game.TakeTurn(Order, selectedTile.Index);
        OnTurnOver(new TurnOverEventArgs(selectedTile));
        return game;
    }

}
