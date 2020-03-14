namespace JustChess.Movements.Contracts
{
    using Common;
    using Board.Contracts;
    using Figures.Contracts;

    public interface IMovement
    {
        void ValidateMove(IFigure figure, IBoard board, Move move);
    }
}
