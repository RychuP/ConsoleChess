namespace ConsoleChess.Renderers
{
    // .NET
    using System;

    // Libraries
    using SadConsole;
    using Microsoft.Xna.Framework;
    using Console = SadConsole.Console;

    // Chess
    using Board.Contracts;
    using Common;
    using Contracts;
    using Figures.Contracts;

    public class ConsoleRenderer : IRenderer
    {
        readonly Console boardConsole;
        readonly Color DarkSquareColor = Color.DarkGray;
        readonly Color LightSquareColor = Color.Gray;
        readonly int consoleHeight, consoleWidth, borderPadding = 4;

        public ConsoleRenderer(Console parent)
        {
            // calculate size for the boardConsole
            consoleHeight = (GlobalConstants.StandardGameTotalBoardRows
                * GlobalConstants.CharactersPerRowPerBoardSquare) 
                + borderPadding;
            consoleWidth = (GlobalConstants.StandardGameTotalBoardCols
                * GlobalConstants.CharactersPerColPerBoardSquare) 
                + borderPadding;

            // load font
            var fontMaster = SadConsole.Global.LoadFont("Fonts/rychu.font");
            var halfSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.Half);

            // create board console
            boardConsole = new Console(consoleWidth, consoleHeight, halfSizedFont)
            {
                Parent = parent
            };
        }

        public void RenderMainMenu()
        {
            
        }

        public void RenderBoard(IBoard board)
        {
            int startX = 0, startY = 0, currentX, currentY;

            PrintBorder(startX, startY, board.TotalRows, board.TotalCols);

            int counter = 1;
            for (int y = 0; y < board.TotalRows; y++)
            {
                for (int x = 0; x < board.TotalCols; x++)
                {
                    int offset = borderPadding / 2;
                    currentX = startX + (x * GlobalConstants.CharactersPerColPerBoardSquare) + offset;
                    currentY = startY + (y * GlobalConstants.CharactersPerRowPerBoardSquare) + offset;

                    Color backgroundColor = (counter % 2 == 0) ? LightSquareColor : DarkSquareColor;

                    var position = Position.FromArrayCoordinates(y, x, board.TotalRows);
                    var figure = board.GetFigureAtPosition(position);
                    PrintFigure(figure, backgroundColor, currentX, currentY);

                    counter++;
                }

                counter++;
            }
        }

        public void PrintErrorMessage(string errorMessage)
        {

        }

        void PrintBorder(int startX, int startY, int boardTotalRows, int boardTotalCols)
        {
            int x, y;

            // border
            var rect = new Rectangle(0, 0, consoleWidth, consoleHeight);
            var cell = new Cell(Color.White, DarkSquareColor, 0);
            boardConsole.DrawBox(rect, cell);

            // column letters above and below board
            int xStart = startX + (GlobalConstants.CharactersPerColPerBoardSquare / 2);
            for (int i = 0; i < boardTotalCols; i++)
            {
                x = xStart + (i * GlobalConstants.CharactersPerRowPerBoardSquare) + borderPadding / 2;

                // top row
                y = startY + 1;
                boardConsole.SetGlyph(x, y, (char)('A' + i));

                // bottom row
                y += boardTotalRows * GlobalConstants.CharactersPerRowPerBoardSquare + 1;
                boardConsole.SetGlyph(x, y, (char)('A' + i));
            }

            // row numbers on the left and right of the board
            int yStart = startY + (GlobalConstants.CharactersPerRowPerBoardSquare / 2);
            for (int i = 0; i < boardTotalRows; i++)
            {
                y = yStart + (i * GlobalConstants.CharactersPerColPerBoardSquare) + borderPadding / 2;

                // left column
                x = startX + 1;
                boardConsole.SetGlyph(x, y, (char)('0' + boardTotalRows - i));

                // right column
                x += boardTotalCols * GlobalConstants.CharactersPerColPerBoardSquare + 1;
                boardConsole.SetGlyph(x, y, (char)('0' + boardTotalRows - i));
            }
        }

        void PrintEmptySquare(Color backgroundColor, int currentX, int currentY)
        {
            for (int y = 0; y < GlobalConstants.CharactersPerRowPerBoardSquare; y++)
            {
                boardConsole.Print(currentX, currentY + y, 
                    new string(' ', GlobalConstants.CharactersPerColPerBoardSquare),
                    Color.White, backgroundColor);
            }
        }

        void PrintFigure(IFigure figure, Color backgroundColor, int currentX, int currentY)
        {
            if (figure is null)
            {
                PrintEmptySquare(backgroundColor, currentX, currentY);
                return;
            }

            for (int y = 0; y < figure.Pattern.GetLength(0); y++)
            {
                for (int x = 0; x < figure.Pattern.GetLength(1); x++)
                {
                    boardConsole.SetGlyph(currentX + x, currentY + y, figure.Pattern[y, x],
                        figure.Color.ToConsoleColor(), backgroundColor);
                }
            }
        }
    }
}
