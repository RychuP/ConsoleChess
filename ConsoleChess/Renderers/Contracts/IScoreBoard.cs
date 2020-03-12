using ConsoleChess.Common;
using ConsoleChess.Players;
using ConsoleChess.Players.Contracts;
using System.Collections.Generic;

namespace ConsoleChess.Renderers.Contracts
{
    public interface IScoreBoard
    {
        void RecordMove(Move move, IPlayer player, IPlayer nextPlayer);

        void Initialize(IList<IPlayer> players);
    }
}
