namespace ConsoleChess.Figures.Contracts
{
    using System.Collections.Generic;
    using SadConsole;
    using Common;
    using Movements.Contracts;

    public abstract class BaseFigure : IFigure
    {
        protected BaseFigure(ChessColor color)
        {
            Color = color;
            Moved = false;
        }

        public abstract ICollection<IMovement> Move(IMovementStrategy strategy);

        public ChessColor Color { get; private set; }

        public int[,] Pattern { get; protected set; }

        public bool Moved { get; set; }

        public char Letter { get; protected set; }
    }
}
