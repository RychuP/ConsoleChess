namespace JustChess.Figures
{
    // .NET
    using System.Collections.Generic;

    // Chess
    using Common;
    using Contracts;
    using Movements.Contracts;

    public class Pawn : BaseFigure, IFigure
    {
        static readonly int[,] pattern = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 0, 64, 0, 0, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 0, 0, 0, 64, 0, 0, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 0, 64, 64, 64, 64, 64, 0, 0, },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        static readonly int[,] pattern2 = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 2, 3, 0, 0, 0 },
            { 0, 0, 0, 4, 64, 5, 0, 0, 0 },
            { 0, 0, 0, 6, 64, 7, 0, 0, 0 },
            { 0, 0, 8, 9, 64, 10, 11, 0, 0 },
            { 0, 0, 12, 64, 64, 64, 13, 0, 0 },
            { 0, 0, 14, 15, 16, 17, 18, 0, 0 },
            { 0, 0, 19, 20, 21, 22, 23, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        public Pawn(ChessColor color) : base(color)
        {
            Pattern[0] = pattern;
            Pattern[1] = pattern2;
            Letter = 'P';
        }

        public override ICollection<IMovement> Move(IMovementStrategy strategy)
        {
            return strategy.GetMovements(GetType().Name);
        }
    }
}
