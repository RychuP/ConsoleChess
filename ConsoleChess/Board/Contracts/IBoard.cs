namespace ConsoleChess.Board.Contracts
{
    using Common;
    using Figures.Contracts;
    using System.Collections.Generic;

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
