namespace ConsoleChess.Engine
{
    // .NET
    using System;
    using System.Collections.Generic;

    // Libraries
    using SadConsole;
    using Microsoft.Xna.Framework;

    // Chess
    using Board;
    using Board.Contracts;
    using Common;
    using ConsoleChess.Renderers;
    using Contracts;
    using Figures.Contracts;
    using Movements.Contracts;
    using Movements.Strategies;
    using Players;
    using Players.Contracts;
    using Renderers.Contracts;
    using Figures;

    public class StandardTwoPlayerEngine : IChessEngine
    {
        readonly IMovementStrategy movementStrategy;
        readonly IRenderer renderer;
        readonly IBoard board;
        readonly IScoreBoard scoreBoard;
        IList<IPlayer> players;
        int currentPlayerIndex = 1;
        IList<Position> selectedPositions;

        public StandardTwoPlayerEngine(ContainerConsole container)
        {
            movementStrategy = new NormalMovementStrategy();
            renderer = new ConsoleRenderer(container);
            selectedPositions = new List<Position>();
            scoreBoard = new ScoreBoard(container);
            board = new Board();

            renderer.Console.MouseMove += MouseClickOnBoard;
        }

        public IEnumerable<IPlayer> Players
        {
            get
            {
                return new List<IPlayer>(players);
            }
        }

        public void Initialize(IGameInitializationStrategy gameInitializationStrategy)
        {
            // TODO: BUG: if players are changed - board is reversed
            players = new List<IPlayer> 
            {
                new Player("Black", ChessColor.Black),
                new Player("White", ChessColor.White)
            };

            gameInitializationStrategy.Initialize(players, board);
            scoreBoard.Initialize(players);
            renderer.RenderBoard(board);
        }

        public void WinningConditions()
        {
            throw new NotImplementedException();
        }

        void ValidateMovements(IFigure figure, IEnumerable<IMovement> availableMovements, Move move)
        {
            var validMoveFound = false;
            var foundException = new Exception();
            foreach (var movement in availableMovements)
            {
                try
                {
                    movement.ValidateMove(figure, board, move);
                    validMoveFound = true;
                    break;
                }
                catch (Exception ex)
                {
                    foundException = ex;
                }
            }

            if (!validMoveFound)
            {
                throw foundException;
            }
        }

        void NextPlayer()
        {
            currentPlayerIndex++;
            if (currentPlayerIndex >= players.Count)
            {
                currentPlayerIndex = 0;
            }
        }

        void CheckIfPlayerOwnsFigure(IPlayer player, IFigure figure, Position from)
        {
            if (figure == null)
            {
                throw new InvalidOperationException(string.Format("Position {0}{1} is empty!", from.Col, from.Row));
            }

            if (figure.Color != player.Color)
            {
                throw new InvalidOperationException(string.Format("Figure at {0}{1} is not yours!", from.Col, from.Row));
            }
        }

        void CheckIfToPositionIsEmpty(IFigure figure, Position to)
        {
            var figureAtPosition = board.GetFigureAtPosition(to);
            if (figureAtPosition != null && figureAtPosition.Color == figure.Color)
            {
                throw new InvalidOperationException(string.Format("You already have a figure at {0}{1}!", to.Col, to.Row));
            }
        }

        void CheckIfKingIsInCheck(King king)
        {

            // check for attacking pawns
            if (king.Color == ChessColor.White)
            {

            }
        }

        void MouseClickOnBoard(object sender, SadConsole.Input.MouseEventArgs e)
        {
            if (e.MouseState.Mouse.LeftClicked)
            {
                Point mousePosition = e.MouseState.CellPosition;
                int column = (mousePosition.X - GlobalConstants.BorderWidth - 1)
                    / GlobalConstants.CharactersPerColPerBoardSquare;
                // prevent invalid coordinates
                if (column >= board.TotalCols) column = board.TotalCols - 1;
                char chessCoordX = (char)('a' + column);

                int row = (mousePosition.Y - GlobalConstants.BorderWidth - 1)
                    / GlobalConstants.CharactersPerRowPerBoardSquare;
                // prevent invalid coordinates
                if (row >= board.TotalRows) row = board.TotalRows - 1;
                int chessCoordY = board.TotalRows - row;

                // get objects
                Position position = new Position(chessCoordY, chessCoordX);
                IPlayer player = players[currentPlayerIndex];
                IFigure figure, capturedFigure;

                // check selected positions
                switch (selectedPositions.Count)
                {
                    case 0:
                        figure = board.GetFigureAtPosition(position);
                        try
                        {
                            CheckIfPlayerOwnsFigure(player, figure, position);
                        }
                        catch (Exception f)
                        {
                            ResetMoves();
                            break;
                        }
                        renderer.HighlightPosition(position, Color.Green);
                        selectedPositions.Add(position);
                        scoreBoard.RecordMove(player, position);
                        break;

                    case 1:
                        Position from = selectedPositions[0];
                        Position to = position;
                        Move move = new Move(from, to);
                        figure = board.GetFigureAtPosition(from);
                        capturedFigure = board.GetFigureAtPosition(to);

                        try
                        {
                            // engine update with a risk of exceptions
                            CheckIfToPositionIsEmpty(figure, position);
                            var availableMovements = figure.Move(movementStrategy);
                            ValidateMovements(figure, availableMovements, move);
                            board.MoveFigureAtPosition(figure, from, to);

                            // TODO: On every move check if we are in check
                            // TODO: Check pawn on last row
                            // TODO: If not castle - move figure 
                            // (check castle - check if castle is valid, check pawn for An-pasan)
                            // TODO: If in check - check checkmate
                            // TODO: If not in check - check draw
                            // TODO: display error messages
                        }
                        catch (Exception f)
                        {
                            scoreBoard.RecordMove(player);
                            ResetMoves();
                            break;
                        }

                        // engine update
                        renderer.RenderBoard(board);
                        if (capturedFigure != null)
                        {
                            player.AddTrophy(capturedFigure);
                        }
                        ResetMoves();
                        NextPlayer();

                        // score board update
                        var nextPlayer = players[currentPlayerIndex];
                        scoreBoard.RecordMove(move, player, nextPlayer);
                        break;

                    default:
                        ResetMoves();
                        break;
                }
            }
        }

        void ResetMoves()
        {
            foreach (var position in selectedPositions)
            {
                renderer.RemoveHighlight(position);
            }
            selectedPositions.Clear();
        }
    }
}
