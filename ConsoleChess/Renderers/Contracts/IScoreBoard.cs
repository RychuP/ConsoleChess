using ConsoleChess.Common;
using ConsoleChess.Players;
using ConsoleChess.Players.Contracts;
using System.Collections.Generic;

namespace ConsoleChess.Renderers.Contracts
{
    public interface IScoreBoard
    {
        void RecordMove(Move move, Player player);

        void Initialize(IList<IPlayer> players);
    }
}
