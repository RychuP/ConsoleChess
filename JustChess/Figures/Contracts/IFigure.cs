namespace JustChess.Figures.Contracts
{
    // .NET
    using System.Collections.Generic;

    // Libraries
    using SadConsole;

    // Chess
    using Common;
    using Movements.Contracts;

    public interface IFigure
    {
        ChessColor Color { get; }

        ICollection<IMovement> Move(IMovementStrategy movementStrategy);

        int[][,] Pattern { get; }

        bool Moved { get; set; }

        char Letter { get; }
    }
}
