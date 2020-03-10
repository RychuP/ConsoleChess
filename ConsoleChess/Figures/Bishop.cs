namespace ConsoleChess.Figures
{
    using System.Collections.Generic;

    using Common;
    using Contracts;
    using Movements.Contracts;

    public class Bishop : BaseFigure, IFigure
    {
        static readonly int[,] pattern = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 0, 219, 0, 0, 0, 0, },
            { 0, 0, 0, 219, 219, 219, 0, 0, 0, },
            { 0, 0, 219, 219, 0, 219, 219, 0, 0, },
            { 0, 0, 219, 0, 0, 0, 219, 0, 0, },
            { 0, 0, 0, 219, 0, 219, 0, 0, 0, },
            { 0, 0, 0, 0, 219, 0, 0, 0, 0, },
            { 0, 219, 219, 219, 0, 219, 219, 219, 0, },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        public Bishop(ChessColor color) : base(color)
        {
            Pattern = pattern;
        }

        public override ICollection<IMovement> Move(IMovementStrategy strategy)
        {
            return strategy.GetMovements(this.GetType().Name);
        }
    }
}
