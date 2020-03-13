namespace ConsoleChess.Figures
{
    // .NET
    using System.Collections.Generic;

    // Chess
    using Common;
    using Contracts;
    using Movements.Contracts;

    public class King : BaseFigure, IFigure
    {
        static readonly int[,] pattern = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 0, 64, 0, 0, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 64, 64, 0, 64, 0, 64, 64, 0, },
            { 0, 64, 64, 64, 0, 64, 64, 64, 0, },
            { 0, 64, 64, 64, 64, 64, 64, 64, 0, },
            { 0, 0, 64, 64, 64, 64, 64, 0, 0, },
            { 0, 0, 64, 64, 64, 64, 64, 0, 0, },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        static readonly int[,] pattern2 = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 0, 172,173, 174, 0, 0, 0 },
            { 0, 0, 175, 176, 177, 178, 179, 0, 0 },
            { 0, 180, 64, 93, 181, 182, 64, 183, 0 },
            { 0, 184, 64, 64, 64, 64, 64, 185, 0 },
            { 0, 186, 64, 64, 64, 64, 64, 187, 0 },
            { 0, 0, 188, 189, 190, 191, 192, 0, 0 },
            { 0, 0, 193, 194, 64, 195, 196, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        public King(ChessColor color) : base(color)
        {
            Pattern[0] = pattern;
            Pattern[1] = pattern2;
            Letter = 'K';
        }

        public override ICollection<IMovement> Move(IMovementStrategy strategy)
        {
            return strategy.GetMovements(GetType().Name);
        }
    }
}
