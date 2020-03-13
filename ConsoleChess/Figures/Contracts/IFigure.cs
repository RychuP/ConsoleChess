namespace ConsoleChess.Figures.Contracts
{
    using System.Collections.Generic;
    using SadConsole;
    using Common;
    using Movements.Contracts;

    public interface IFigure
    {
        ChessColor Color { get; }

        ICollection<IMovement> Move(IMovementStrategy movementStrategy);

        int[,] Pattern { get; }

        bool Moved { get; set; }

        char Letter { get; }
    }
}
