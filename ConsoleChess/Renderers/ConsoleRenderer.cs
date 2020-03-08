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
    using Common.Console;
    using Contracts;

    public class ConsoleRenderer : IRenderer
    {
        readonly Color DarkSquareColor = Color.DarkGray;
        readonly Color LightSquareColor = Color.Gray;
        readonly int consoleHeight, consoleWidth, borderPadding = 4;
        readonly Console boardConsole;

        public ConsoleRenderer(Console parent)
        {
            // calculate size for the boardConsole
            consoleHeight = (GlobalConstants.StandardGameTotalBoardRows
                * ConsoleConstants.CharactersPerRowPerBoardSquare) + borderPadding;
            consoleWidth = (GlobalConstants.StandardGameTotalBoardCols
                * ConsoleConstants.CharactersPerColPerBoardSquare) + borderPadding;

            // load font
            var fontMaster = SadConsole.Global.LoadFont("fonts/C64.font");
            var halfSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.Half);

            // create console
            boardConsole = new Console(consoleWidth, consoleHeight, halfSizedFont)
            {
                Parent = parent
            };

            // test
            PrintBorder(0, 0, 8, 8);
        }

        public void RenderMainMenu()
        {
            
        }

        public void RenderBoard(IBoard board)
        {
            /*
            var currentRowPrint = 0;
            var currentColPrint = 0;

            PrintBorder(0, 0, board.TotalRows, board.TotalCols);

            Console.BackgroundColor = ConsoleColor.White;
            int counter = 1;
            for (int top = 0; top < board.TotalRows; top++)
            {
                for (int left = 0; left < board.TotalCols; left++)
                {
                    currentColPrint = startRowPrint + (left * ConsoleConstants.CharactersPerColPerBoardSquare);
                    currentRowPrint = startColPrint + (top * ConsoleConstants.CharactersPerRowPerBoardSquare);

                    ConsoleColor backgroundColor;
                    if (counter % 2 == 0)
                    {
                        backgroundColor = DarkSquareColor;
                        Console.BackgroundColor = DarkSquareColor;
                    }
                    else
                    {
                        backgroundColor = LightSquareColor;
                        Console.BackgroundColor = LightSquareColor;
                    }

                    var position = Position.FromArrayCoordinates(top, left, board.TotalRows);

                    var figure = board.GetFigureAtPosition(position);
                    ConsoleHelpers.PrintFigure(figure, backgroundColor, currentRowPrint, currentColPrint);

                    counter++;
                }

                counter++;
            }
            */
        }

        public void PrintErrorMessage(string errorMessage)
        {

        }

        private void PrintBorder(int startRowPrint, int startColPrint, int boardTotalRows, int boardTotalCols)
        {
            int x, y;

            // border
            var rect = new Rectangle(0, 0, consoleWidth, consoleHeight);
            var cell = new Cell(Color.White, DarkSquareColor, 0);
            boardConsole.DrawBox(rect, cell);

            // column letters above and below board
            int xStart = startColPrint + (ConsoleConstants.CharactersPerColPerBoardSquare / 2);
            for (int i = 0; i < boardTotalCols; i++)
            {
                x = xStart + (i * ConsoleConstants.CharactersPerRowPerBoardSquare) + borderPadding / 2;

                // top row
                y = startRowPrint + 1;
                boardConsole.SetGlyph(x, y, (char)('A' + i));

                // bottom row
                y += boardTotalRows * ConsoleConstants.CharactersPerRowPerBoardSquare + 1;
                boardConsole.SetGlyph(x, y, (char)('A' + i));
            }

            // row numbers on the left and right of the board
            int charZero = (char)'0';
            int yStart = startRowPrint + (ConsoleConstants.CharactersPerRowPerBoardSquare / 2);
            for (int i = 0; i < boardTotalRows; i++)
            {
                y = yStart + (i * ConsoleConstants.CharactersPerColPerBoardSquare) + borderPadding / 2;

                // left column
                x = startColPrint + 1;
                boardConsole.SetGlyph(x, y, charZero + boardTotalRows - i);

                // right column
                x += boardTotalCols * ConsoleConstants.CharactersPerColPerBoardSquare + 1;
                boardConsole.SetGlyph(x, y, charZero + boardTotalRows - i);
            }
        }
    }
}
