namespace ConsoleChess.Figures
{
    // .NET
    using System.Collections.Generic;

    // Chess
    using Common;
    using Contracts;
    using Movements.Contracts;

    public class Knight : BaseFigure, IFigure
    {
        static readonly int[,] pattern = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 0, 64, 64, 0, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 64, 0, 0, },
            { 0, 0, 64, 64, 64, 0, 64, 0, 0, },
            { 0, 0, 0, 64, 0, 64, 64, 0, 0, },
            { 0, 0, 0, 0, 64, 64, 64, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 0, 64, 64, 64, 64, 64, 0, 0, },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        static readonly int[,] pattern2 = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 24, 25, 26, 0, 0, 0 },
            { 0, 0, 27, 28, 64, 64, 29, 0, 0 },
            { 0, 30, 9, 31, 32, 64, 33, 0, 0 },
            { 0, 34, 64, 64, 35, 64, 36, 0, 0 },
            { 0, 37, 38, 39, 64, 64, 64, 0, 0 },
            { 0, 0, 40, 41, 42, 43, 44, 0, 0 },
            { 0, 0, 45, 46, 47, 47, 80, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        public Knight(ChessColor color) : base(color)
        {
            Pattern = pattern2;
            Letter = 'N';
        }

        public override ICollection<IMovement> Move(IMovementStrategy strategy)
        {
            return strategy.GetMovements(GetType().Name);
        }
    }
}
