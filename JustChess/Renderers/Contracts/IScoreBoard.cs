namespace JustChess.Renderers.Contracts
{
    // .NET
    using System.Collections.Generic;

    // Chess
    using Common;
    using Players;
    using Players.Contracts;

    public interface IScoreBoard
    {
        void RecordMove(Move move, IPlayer player, IPlayer nextPlayer);

        void RecordMove(IPlayer currentPlayer, Position? position = null);

        void Initialize(IList<IPlayer> players);
    }
}
