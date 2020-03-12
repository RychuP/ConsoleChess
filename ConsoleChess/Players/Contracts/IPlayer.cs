namespace ConsoleChess.Players.Contracts
{
    using Common;
    using Figures;
    using Figures.Contracts;
    using System.Collections.Generic;

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
