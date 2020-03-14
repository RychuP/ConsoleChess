namespace JustChess.Figures.Contracts
{
    // .NET
    using System.Collections.Generic;

    // Libraries
    using SadConsole;

    // Chess
    using Common;
    using Movements.Contracts;

    public abstract class BaseFigure : IFigure
    {
        protected BaseFigure(ChessColor color)
        {
            Color = color;
            Moved = false;
            Pattern = new int[2][,];
        }

        public abstract ICollection<IMovement> Move(IMovementStrategy strategy);

        public ChessColor Color { get; private set; }

        public int[][,] Pattern { get; protected set; }

        public bool Moved { get; set; }

        public char Letter { get; protected set; }
    }
}
