namespace ConsoleChess.Engine
{
    // .NET
    using System;
    using System.Collections.Generic;

    // Libraries
    using SadConsole;

    // Chess
    using Board;
    using Board.Contracts;
    using Common;
    using ConsoleChess.InputProviders;
    using ConsoleChess.Renderers;
    using Contracts;
    using Figures.Contracts;
    using InputProviders.Contracts;
    using Movements.Contracts;
    using Movements.Strategies;
    using Players;
    using Players.Contracts;
    using Renderers.Contracts;

    public class StandardTwoPlayerEngine : IChessEngine
    {
        readonly IMovementStrategy movementStrategy;
        readonly IRenderer renderer;
        readonly IInputProvider input;
        readonly IBoard board;
        readonly IScoreBoard scoreBoard;
        IList<IPlayer> players;
        int currentPlayerIndex;


        public StandardTwoPlayerEngine(ContainerConsole container)
        {
            movementStrategy = new NormalMovementStrategy();
            renderer = new ConsoleRenderer(container);
            scoreBoard = new ScoreBoard(container);
            input = new ConsoleInputProvider();
            board = new Board();
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
            // TODO: remove using JustChess.Players and use the input for players
            // TODO: BUG: if players are changed - board is reversed
            players = new List<IPlayer> 
            {
                new Player("Black", ChessColor.Black),
                new Player("White", ChessColor.White)
            }; // input.GetPlayers(GlobalConstants.StandardGameNumberOfPlayers);

            SetFirstPlayerIndex();
            gameInitializationStrategy.Initialize(players, board);
            scoreBoard.Initialize(players);
            renderer.RenderBoard(board);
        }

        public void Start()
        {
            while (true)
            {
                IFigure figure = null;
                try
                {
                    var player = GetNextPlayer();
                    var move = input.GetNextPlayerMove(player);
                    var from = move.From;
                    var to = move.To;
                    figure = board.GetFigureAtPosition(from);
                    CheckIfPlayerOwnsFigure(player, figure, from);
                    CheckIfToPositionIsEmpty(figure, to);

                    var availableMovements = figure.Move(movementStrategy);
                    ValidateMovements(figure, availableMovements, move);

                    board.MoveFigureAtPosition(figure, from, to);
                    renderer.RenderBoard(board);

                    // TODO: On every move check if we are in check
                    // TODO: Check pawn on last row
                    // TODO: If not castle - move figure (check castle - check if castle is valid, check pawn for An-pasan)
                    // TODO: If in check - check checkmate
                    // TODO: If not in check - check draw
                    // TODO: Continue
                }
                catch (Exception ex)
                {
                    currentPlayerIndex--;
                    renderer.PrintErrorMessage(string.Format(ex.Message, figure.GetType().Name));
                }
            }
        }

        public void WinningConditions()
        {
            throw new NotImplementedException();
        }

        private void ValidateMovements(IFigure figure, IEnumerable<IMovement> availableMovements, Move move)
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

        private void SetFirstPlayerIndex()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Color == ChessColor.White)
                {
                    currentPlayerIndex = i - 1;
                    return;
                }
            }
        }

        private IPlayer GetNextPlayer()
        {
            currentPlayerIndex++;
            if (currentPlayerIndex >= players.Count)
            {
                currentPlayerIndex = 0;
            }

            return players[currentPlayerIndex];
        }

        private void CheckIfPlayerOwnsFigure(IPlayer player, IFigure figure, Position from)
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

        private void CheckIfToPositionIsEmpty(IFigure figure, Position to)
        {
            var figureAtPosition = board.GetFigureAtPosition(to);
            if (figureAtPosition != null && figureAtPosition.Color == figure.Color)
            {
                throw new InvalidOperationException(string.Format("You already have a figure at {0}{1}!", to.Col, to.Row));
            }
        }
    }
}
