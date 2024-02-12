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
/// <summary>
/// This bot will prioritize playing the tile opposite to where the player played as seen below.
/// If forced to go first, this bot will choose the middle tile if there is one.
/// If the bot cannot choose an opposite tile or the center tile (ie. the player chose center or an even sized board) it will randomly choose from the available tiles.
/// 
/// If the player chooses tile 'X', the bot will choose tile 'Y'
/// X | X | X
/// X |   | Y
/// Y | Y | Y
/// </summary>
public class NoYouBot : ITicTacToeBot
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
    public TileOwner Order
    {
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
    private readonly Random _random = new(DateTime.UtcNow.Microsecond * DateTime.UtcNow.Millisecond);
    private TileOwner _order;
    public NoYouBot(TileOwner order)
    {
        _order = order;
    }
    public ITicTacToeBot New(TileOwner order) => new NoYouBot(order);
    public Board TakeTurn(Board game, Tile? tilePlayed)
    {
        var unclaimedTiles = (from Tile tile in game.Positions
                              where tile.Owner is TileOwner.Unclaimed
                              select tile).ToList();
        if (unclaimedTiles.Count == 0) return game;//if there are no unclaimed tiles then the game is over
        if (unclaimedTiles.Count == 1)//if there's only 1 option, play it
        {
            var tiles = game.Positions.Cast<Tile>();
            var tile = tiles.First(Tile => Tile.Owner is TileOwner.Unclaimed);
            game.TakeTurn(Order, tile.Coords);
            OnTurnOver(new TurnOverEventArgs(tile));
            return game;
        }

        var randTile = unclaimedTiles[_random.Next(unclaimedTiles.Count - 1)].Coords;
        Tile? choice = null;

        if (tilePlayed is null)//if the bot is going first
        {
            int center = (int)Math.Floor(game.BoardSize / 2f);
            if (!game.IsEven)//play center if possible
                choice ??= game.Positions[center, center];

            choice ??= game.Positions[randTile.X, randTile.Y];
        }

        //opposites
        try { choice ??= GetOpposites(game, tilePlayed!).First(tile => tile.Owner is TileOwner.Unclaimed); } catch (Exception) { };

        //center
        if (!game.IsEven &&
            game.Positions[(int)game.Center, (int)game.Center].Owner is TileOwner.Unclaimed)
            choice ??= game.Positions[(int)game.Center, (int)game.Center];

        //random if all else fails
        choice ??= game.Positions[randTile.X, randTile.Y];

        game.TakeTurn(Order, choice.Coords);
        OnTurnOver(new TurnOverEventArgs(choice));
        return game;
    }

    private static List<Tile> GetOpposites(Board game, Tile tilePlayed)
    {
        var output = new List<Tile>();
        if (tilePlayed is null) return output;

        if (!game.IsEven && tilePlayed.Coords.X == game.Center && tilePlayed.Coords.Y == game.Center)//if tile played is the center tile on an odd sized board, there is no diagonal
            return output;

        if (tilePlayed.Coords.X == tilePlayed.Coords.Y || tilePlayed.Coords.X + tilePlayed.Coords.Y == game.BoardSize - 1)//if tile played was on a diagonal, the opposite diagonal takes priority
            output.Add(game.Positions[game.AdjustedBoardSize - tilePlayed.Coords.X, game.AdjustedBoardSize - tilePlayed.Coords.Y]);

        if(Math.Abs(tilePlayed.Coords.X - (game.AdjustedBoardSize - tilePlayed.Coords.X)) > Math.Abs(tilePlayed.Coords.Y - (game.AdjustedBoardSize - tilePlayed.Coords.Y)))//prioritize a farther opposite over a nearer one.
        {
            output.Add(game.Positions[game.AdjustedBoardSize - tilePlayed.Coords.X, tilePlayed.Coords.Y]);
            output.Add(game.Positions[tilePlayed.Coords.X, game.AdjustedBoardSize - tilePlayed.Coords.Y]);
            return output;
        }
        output.Add(game.Positions[tilePlayed.Coords.X, game.AdjustedBoardSize - tilePlayed.Coords.Y]);
        output.Add(game.Positions[game.AdjustedBoardSize - tilePlayed.Coords.X, tilePlayed.Coords.Y]);
        return output;
    }
}

