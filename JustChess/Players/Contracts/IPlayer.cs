namespace JustChess.Players.Contracts
{
    // .NET
    using System.Collections.Generic;

    // Chess
    using Common;
    using Figures;
    using Figures.Contracts;

    public interface IPlayer
    {
        string Name { get; }

        ChessColor Color { get; }

        void AddFigure(IFigure figure);

        void RemoveFigure(IFigure figure);

        IList<IFigure> Trophies { get; }

        void AddTrophy(IFigure figure);

        void ResetTrophies();
    }
}
