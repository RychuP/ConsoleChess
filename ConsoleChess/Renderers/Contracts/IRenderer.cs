namespace ConsoleChess.Renderers.Contracts
{
    // .NET
    using System;

    // Libraries
    using SadConsole.Input;
    using Console = SadConsole.Console;
    using Microsoft.Xna.Framework;

    // Chess
    using Common;
    using Board.Contracts;

    public interface IRenderer
    {
        void RenderMainMenu();

        void RenderBoard(IBoard board);

        void PrintErrorMessage(string errorMessage);

        Console Console { get; }

        void RemoveHighlight(Position pos);

        void HighlightPosition(Position pos, Color col);

        Console ShowPiecePromotion(ChessColor col);
    }
}
