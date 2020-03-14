namespace JustChess.Figures
{
    // .NET
    using System.Collections.Generic;

    // Chess
    using Common;
    using Contracts;
    using Movements.Contracts;

    public class Bishop : BaseFigure, IFigure
    {
        static readonly int[,] pattern = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 0, 64, 0, 0, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 0, 64, 64, 0, 64, 64, 0, 0, },
            { 0, 0, 64, 0, 0, 0, 64, 0, 0, },
            { 0, 0, 0, 64, 0, 64, 0, 0, 0, },
            { 0, 0, 0, 0, 64, 0, 0, 0, 0, },
            { 0, 64, 64, 64, 0, 64, 64, 64, 0, },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        static readonly int[,] pattern2 = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 81, 82, 83, 0, 0, 0 },
            { 0, 0, 30, 84, 64, 85, 86, 0, 0 },
            { 0, 0, 87, 88, 89, 90, 91, 0, 0 },
            { 0, 0, 92, 93, 94, 95, 96, 0, 0 },
            { 0, 0, 97, 98, 99, 100, 101, 0, 0 },
            { 0, 102, 103, 104, 64, 105, 106, 107, 0 },
            { 0, 108, 109, 110, 111, 109, 112, 113, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        public Bishop(ChessColor color) : base(color)
        {
            Pattern[0] = pattern;
            Pattern[1] = pattern2;
            Letter = 'B';
        }

        public override ICollection<IMovement> Move(IMovementStrategy strategy)
        {
            return strategy.GetMovements(GetType().Name);
        }
    }
}
