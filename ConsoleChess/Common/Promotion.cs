namespace ConsoleChess.Common
{
    using Figures.Contracts;

    class Promotion
    {
        public Promotion(IFigure figure, Position position)
        {
            Pawn = figure;
            Position = position;
        }
        public IFigure Pawn { get; }
        public Position Position { get; }
    }
}
