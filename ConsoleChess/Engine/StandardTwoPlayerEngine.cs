namespace ConsoleChess.Engine
{
    // .NET
    using System;
    using System.Collections.Generic;

    // Libraries
    using Console = SadConsole.Console;

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
        IList<IPlayer> players;
        int currentPlayerIndex;


        public StandardTwoPlayerEngine(Console container)
        {
            movementStrategy = new NormalMovementStrategy();
            renderer = new ConsoleRenderer(container);
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
                new Player("Gosho", ChessColor.Black),
                new Player("Pesho", ChessColor.White)
            }; // this.input.GetPlayers(GlobalConstants.StandardGameNumberOfPlayers);

            SetFirstPlayerIndex();
            gameInitializationStrategy.Initialize(players, board);
            renderer.RenderBoard(board);
        }

        public void Start()
        {
            while (true)
            {
                IFigure figure = null;
                try
                {
                    var player = this.GetNextPlayer();
                    var move = this.input.GetNextPlayerMove(player);
                    var from = move.From;
                    var to = move.To;
                    figure = this.board.GetFigureAtPosition(from);
                    this.CheckIfPlayerOwnsFigure(player, figure, from);
                    this.CheckIfToPositionIsEmpty(figure, to);

                    var availableMovements = figure.Move(this.movementStrategy);
                    this.ValidateMovements(figure, availableMovements, move);

                    this.board.MoveFigureAtPosition(figure, from, to);
                    this.renderer.RenderBoard(this.board);

                    // TODO: On every move check if we are in check
                    // TODO: Check pawn on last row
                    // TODO: If not castle - move figure (check castle - check if castle is valid, check pawn for An-pasan)
                    // TODO: If in check - check checkmate
                    // TODO: If not in check - check draw
                    // TODO: Continue
                }
                catch (Exception ex)
                {
                    this.currentPlayerIndex--;
                    this.renderer.PrintErrorMessage(string.Format(ex.Message, figure.GetType().Name));
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
                    movement.ValidateMove(figure, this.board, move);
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
            for (int i = 0; i < this.players.Count; i++)
            {
                if (this.players[i].Color == ChessColor.White)
                {
                    this.currentPlayerIndex = i - 1;
                    return;
                }
            }
        }

        private IPlayer GetNextPlayer()
        {
            this.currentPlayerIndex++;
            if (this.currentPlayerIndex >= this.players.Count)
            {
                this.currentPlayerIndex = 0;
            }

            return this.players[this.currentPlayerIndex];
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
            var figureAtPosition = this.board.GetFigureAtPosition(to);
            if (figureAtPosition != null && figureAtPosition.Color == figure.Color)
            {
                throw new InvalidOperationException(string.Format("You already have a figure at {0}{1}!", to.Col, to.Row));
            }
        }
    }
}
