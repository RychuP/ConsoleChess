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
        readonly IList<Position> selectedPositions;
        IList<IPlayer> players;
        int currentPlayerIndex = 1;

        public StandardTwoPlayerEngine(ContainerConsole container)
        {
            movementStrategy = new NormalMovementStrategy();
            renderer = new ConsoleRenderer(container);
            selectedPositions = new List<Position>();
            scoreBoard = new ScoreBoard(container);
            board = new Board();

            // main event that drives the game
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

        (Rook Figure, Position Position, Position Destination) CheckIfCastlingPossible(ChessColor color, MoveType moveType)
        {
            char castlingDirection = (char)(moveType == MoveType.CastleKingSide ? 1 : -1);

            // get the king and rook figures
            char kingsCol = 'e';
            char rooksCol = moveType == MoveType.CastleKingSide ? 'h' : 'a';
            int row = color == ChessColor.White ? 1 : 8;
            var rooksPosition = new Position(row, rooksCol);
            var kingsPosition = new Position(row, kingsCol);
            Rook rook = board.GetFigureAtPosition(rooksPosition) as Rook;
            King king = board.GetFigureAtPosition(kingsPosition) as King;

            // check if they moved
            if (rook is null || king is null || rook.Moved || king.Moved)
            {
                throw new Exception("Castling forfeited. Figures have moved.");
            }

            // check if spaces between the king and the rook are empty
            char colNextToKing = (char)(kingsCol + castlingDirection);
            var positionNextToKing = new Position(row, colNextToKing);
            var testMove = new Move(rooksPosition, positionNextToKing);
            var availableMovements = rook.Move(movementStrategy);
            ValidateMovements(rook, availableMovements, testMove);

            // check if king is in check and squares that the king passes over are not under attack
            if (KingIsUnderAttack(color, kingsPosition, castlingDirection)) 
            {
                throw new Exception("Castling not possible. Either king or squares that the king passes over are under attack!");
            }

            return (rook, rooksPosition, positionNextToKing);
        }

        // checks if king is under attack and if the castling direction has been passed, also checks two adjecent squares
        bool KingIsUnderAttack(ChessColor color, Position testPosition, char? castlingDirection = null)
        {
            var oppositeColor = color == ChessColor.White ? ChessColor.Black : ChessColor.White;
            IDictionary<IFigure, Position> oppositeArmy = board.GetOppositeArmy(oppositeColor);

            int noOfTestPositions = castlingDirection.HasValue ? 3 : 1;
            for (int i = 0; i < noOfTestPositions; i ++)
            {
                foreach (var chessPiece in oppositeArmy)
                {
                    var figure = chessPiece.Key;
                    var attackingFigurePosition = chessPiece.Value;
                    try
                    {
                        var move = new Move(attackingFigurePosition, testPosition);
                        var availableMovements = figure.Move(movementStrategy);
                        ValidateMovements(figure, availableMovements, move);
                        return true;
                    }
                    catch
                    {
                        continue;
                    }
                }

                // check adjecent positions during castling
                if (castlingDirection.HasValue)
                {
                    testPosition = new Position(testPosition.Row, (char)(testPosition.Col + castlingDirection.Value));
                }
            }
            
            return false;
        }

        void MouseClickOnBoard(object sender, SadConsole.Input.MouseEventArgs e)
        {
            if (e.MouseState.Mouse.LeftClicked)
            {
                // convert mose x, y into chess coors of the board square
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

                // get objects relating to the selected board square
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
                        catch
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
                            // TODO: Check pawn on last row
                            // TODO: If not castle - move figure 
                            // (check castle - check if castle is valid, check pawn for An-pasan)
                            // TODO: If in check - check checkmate
                            // TODO: If not in check - check draw
                            // TODO: display error messages

                            // check if castling
                            int castlingDirection = from.Col - to.Col;
                            if (figure is King && Math.Abs(castlingDirection) == 2)
                            {
                                var moveType = castlingDirection < 0 ? MoveType.CastleKingSide : MoveType.CastleQueenSide;

                                // check validity and get the rook
                                var rook = CheckIfCastlingPossible(figure.Color, moveType);

                                // move figures
                                board.MoveFigureAtPosition(figure, from, to);
                                board.MoveFigureAtPosition(rook.Figure, rook.Position, rook.Destination);

                                // set move type for the score board
                                move.Type = moveType;
                            }

                            // normal move
                            else
                            {
                                CheckIfToPositionIsEmpty(figure, position);
                                var availableMovements = figure.Move(movementStrategy);
                                ValidateMovements(figure, availableMovements, move);
                                board.MoveFigureAtPosition(figure, from, to);

                                // revert the move if king is in checked position
                                var kingsPosition = board.GetKingsPosition(figure.Color);
                                if (KingIsUnderAttack(figure.Color, kingsPosition))
                                {
                                    board.MoveFigureAtPosition(figure, to, from);
                                    scoreBoard.RecordMove(player);
                                    ResetMoves();
                                    break;
                                }

                                // promotion
                                int pawnOnlastRow = figure.Color == ChessColor.White ? 8 : 1;
                                if (figure is Pawn && to.Row == pawnOnlastRow)
                                {

                                }

                                // letter for the score board
                                move.FigureLetter = figure.Letter;

                                // check if a figure has been captured
                                if (capturedFigure != null)
                                {
                                    player.AddTrophy(capturedFigure);
                                    move.Type = MoveType.Capture;
                                }
                            }
                        }
                        catch
                        {
                            scoreBoard.RecordMove(player);
                            ResetMoves();
                            break;
                        }

                        // engine update
                        renderer.RenderBoard(board);
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

        // resets the temporary list of clicked positions on the board
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
