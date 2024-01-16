using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.AI.Interfaces;
using TicTacToe.Core;
using static TicTacToe.Core.Tile;

namespace TicTacToe.AI;
public class BadBot(TileOwner order) : ITicTacToeBot
{
    public Board TakeTurn(Board game, Tile tilePlayed) => PickTile(game);

    private Board PickTile(Board game)
    {
        List<Tile> unclaimedTiles = new();

        foreach(var tile in game.Positions)
            if(tile.Owner is Tile.TileOwner.Unclaimed) unclaimedTiles.Add(tile);

        var rand = new Random();

        game.TakeTurn(order, rand.Next(0, unclaimedTiles.Count));
        return game;

    }
}
