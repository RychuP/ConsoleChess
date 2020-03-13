namespace ConsoleChess.Figures
{
    // .NET
    using System.Collections.Generic;

    // Chess
    using Common;
    using Contracts;
    using Movements.Contracts;

    public class Rook : BaseFigure, IFigure
    {
        static readonly int[,] pattern = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 64, 0, 64, 0, 64, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 0, 0, 64, 64, 64, 0, 0, 0, },
            { 0, 0, 64, 64, 64, 64, 64, 0, 0, },
            { 0, 0, 64, 64, 64, 64, 64, 0, 0, },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        static readonly int[,] pattern2 = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            { 0, 0, 114, 115, 116, 117, 118, 0, 0 },
            { 0, 0, 119, 64, 64, 64, 120, 0, 0 },
            { 0, 0, 121, 64, 64, 64, 122, 0, 0 },
            { 0, 0, 123, 64, 64, 64, 124, 0, 0 },
            { 0, 125, 126, 64, 64, 64, 127, 0, 0 },
            { 0, 128, 129, 130, 131, 132, 133, 0, 0 },
            { 0, 134, 135, 136, 137, 138, 139, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, }
        };

        public Rook(ChessColor color) : base(color)
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
