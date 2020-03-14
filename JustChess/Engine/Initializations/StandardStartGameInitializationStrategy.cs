namespace JustChess.Engine.Initializations
{
    // .NET
    using System;
    using System.Collections.Generic;

    // Chess
    using Board.Contracts;
    using Common;
    using Contracts;
    using Figures;
    using Figures.Contracts;
    using Players.Contracts;

    public class StandardStartGameInitializationStrategy : IGameInitializationStrategy
    {
        const int BoardTotalRowsAndCols = 8;

        readonly IList<Type> figureTypes;

        public StandardStartGameInitializationStrategy()
        {
            figureTypes = new List<Type>
            {
                typeof(Rook),
                typeof(Knight),
                typeof(Bishop),
                typeof(Queen),
                typeof(King),
                typeof(Bishop),
                typeof(Knight),
                typeof(Rook)
            };
        }

        public void Initialize(IList<IPlayer> players, IBoard board)
        {
            ValidateStrategy(players, board);

            var firstPlayer = players[0];
            var secondPlayer = players[1];

            AddArmyToBoardRow(firstPlayer, board, 8);
            AddPawnsToBoardRow(firstPlayer, board, 7);

            AddPawnsToBoardRow(secondPlayer, board, 2);
            AddArmyToBoardRow(secondPlayer, board, 1);
        }

        private void AddPawnsToBoardRow(IPlayer player, IBoard board, int chessRow)
        {
            if (chessRow > BoardTotalRowsAndCols || chessRow < 1)
            {
                throw new Exception("Chess row out of bounds!");
            }

            for (int i = 0; i < BoardTotalRowsAndCols; i++)
            {
                var pawn = new Pawn(player.Color);
                player.AddFigure(pawn);
                var position = new Position(chessRow, (char)(i + 'a'));
                board.AddFigure(pawn, position);
            }
        }

        private void AddArmyToBoardRow(IPlayer player, IBoard board, int chessRow)
        {
            for (int i = 0; i < BoardTotalRowsAndCols; i++)
            {
                var figureType = this.figureTypes[i];
                var figureInstance = (IFigure)Activator.CreateInstance(figureType, player.Color);
                player.AddFigure(figureInstance);
                var position = new Position(chessRow, (char)(i + 'a'));
                board.AddFigure(figureInstance, position);
            }
        }

        private void ValidateStrategy(ICollection<IPlayer> players, IBoard board)
        {
            if (players.Count != GlobalConstants.StandardGameNumberOfPlayers)
            {
                throw new InvalidOperationException("Standard Start Game Initialization Strategy needs exactly two players!");
            }

            if (board.TotalRows != BoardTotalRowsAndCols || board.TotalCols != BoardTotalRowsAndCols)
            {
                throw new InvalidOperationException("Standard Start Game Initialization Strategy needs 8x8 board!");
            }
        }
    }
}
