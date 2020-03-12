namespace ConsoleChess.Renderers.Contracts
{
    using SadConsole;
    using Board.Contracts;
    using ConsoleChess.Common;
    using Microsoft.Xna.Framework;

    public interface IRenderer
    {
        void RenderMainMenu();

        void RenderBoard(IBoard board);

        void PrintErrorMessage(string errorMessage);

        Console Console { get; }

        void RemoveHighlight(Position p);

        void HighlightPosition(Position p, Color c);
    }
}
