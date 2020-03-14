namespace JustChess.Board
{
    // .NET
    using System;
    using System.Collections.Generic;

    // Chess
    using Common;
    using Contracts;
    using Figures;
    using Figures.Contracts;

    public class Board : IBoard
    {
        IFigure[,] board;

        public Board(
            int rows = GlobalConstants.StandardGameTotalBoardRows,
            int cols = GlobalConstants.StandardGameTotalBoardCols)
        {
            TotalRows = rows;
            TotalCols = cols;
            Initialize();
        }

        public int TotalRows { get; private set; }

        public int TotalCols { get; private set; }

        public void Initialize()
        {
            board = new IFigure[TotalRows, TotalCols];
        }

        public void AddFigure(IFigure figure, Position position)
        {
            ObjectValidator.CheckIfObjectIsNull(figure, GlobalErrorMessages.NullFigureErrorMessage);
            Position.CheckIfValid(position);

            int arrRow = GetArrayRow(position.Row);
            int arrCol = GetArrayCol(position.Col);
            board[arrRow, arrCol] = figure;
        }

        public void RemoveFigure(Position position)
        {
            Position.CheckIfValid(position);
            
            int arrRow = GetArrayRow(position.Row);
            int arrCol = GetArrayCol(position.Col);
            board[arrRow, arrCol] = null;
        }

        public IFigure GetFigureAtPosition(Position position)
        {
            int arrRow = GetArrayRow(position.Row);
            int arrCol = GetArrayCol(position.Col);
            return board[arrRow, arrCol];
        }

        public void MoveFigureAtPosition(IFigure figure, Position from, Position to)
        {
            int arrFromRow = GetArrayRow(from.Row);
            int arrFromCol = GetArrayCol(from.Col);
            board[arrFromRow, arrFromCol] = null;

            int arrToRow = GetArrayRow(to.Row);
            int arrToCol = GetArrayCol(to.Col);
            board[arrToRow, arrToCol] = figure;

            figure.Moved = true;
        }

        public Position GetKingsPosition(ChessColor color)
        {
            int rowCount = board.GetLength(0);
            int colCount = board.GetLength(1);

            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    var figure = board[y, x];
                    if (figure is King && figure.Color.Equals(color))
                    {
                        return Position.FromArrayCoordinates(y, x, TotalRows);
                    }
                }
            }

            throw new Exception("Unable to find the king figure with color: " + color);
        }

        public IDictionary<IFigure, Position> GetOppositeArmy(ChessColor color)
        {
            int rowCount = board.GetLength(0);
            int colCount = board.GetLength(1);
            var army = new Dictionary<IFigure, Position>();

            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    var figure = board[y, x];
                    if (figure != null && figure.Color.Equals(color))
                    {
                        var position = Position.FromArrayCoordinates(y, x, TotalRows);
                        army.Add(figure, position);
                    }
                }
            }

            return army;
        }

        int GetArrayRow(int chessRow)
        {
            return TotalRows - chessRow;
        }

        int GetArrayCol(char chessCol)
        {
            return chessCol - 'a';
        }
    }
}
