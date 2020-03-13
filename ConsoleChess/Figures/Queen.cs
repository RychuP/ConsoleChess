namespace ConsoleChess.Figures
{
    // .NET
    using System.Collections.Generic;

    // Chess
    using Common;
    using Contracts;
    using Movements.Contracts;

    public class Queen : BaseFigure, IFigure
    {
        static readonly int[,] pattern = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 0, 64, 0, 0, 0, 0, },
            { 0, 0, 64, 0, 64, 0, 64, 0, 0, },
            { 0, 0, 0, 64, 0, 64, 0, 0, 0, },
            { 0, 64, 0, 64, 64, 64, 0, 64, 0, },
            { 0, 0, 64, 0, 64, 0, 64, 0, 0, },
            { 0, 0, 64, 64, 0, 64, 64, 0, 0, },
            { 0, 0, 64, 64, 64, 64, 64, 0, 0, },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        static readonly int[,] pattern2 = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 140, 141, 0, 142, 143,0, 0 },
            { 0, 0, 144, 145, 0, 146, 147, 0, 0 },
            { 0, 148, 149, 150, 151, 152, 153, 154, 0 },
            { 0, 155, 156, 157, 158, 64, 159, 160, 0 },
            { 0, 0, 161, 64, 64, 64, 162, 0, 0 },
            { 0, 0, 163, 164, 165, 166, 167, 0, 0 },
            { 0, 0, 168, 169, 137, 170, 171, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        public Queen(ChessColor color) : base(color)
        {
            Pattern = pattern2;
            Letter = 'Q';
        }

        public override ICollection<IMovement> Move(IMovementStrategy strategy)
        {
            return strategy.GetMovements(GetType().Name);
        }
    }
}
