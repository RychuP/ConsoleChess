namespace JustChess.Board.Contracts
{
    // .NET
    using System.Collections.Generic;

    // Chess
    using Common;
    using Figures.Contracts;

    public interface IBoard
    {
        int TotalRows { get; }

        int TotalCols { get; }

        void AddFigure(IFigure figure, Position position);

        void RemoveFigure(Position position);

        IFigure GetFigureAtPosition(Position position);

        void MoveFigureAtPosition(IFigure figure, Position from, Position to);

        Position GetKingsPosition(ChessColor color);

        IDictionary<IFigure, Position> GetOppositeArmy(ChessColor color);

        void Initialize();
    }
}
