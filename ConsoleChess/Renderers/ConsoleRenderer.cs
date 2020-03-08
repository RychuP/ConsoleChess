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
        readonly Console boardConsole;
        readonly Color DarkSquareColor = Color.DarkGray;
        readonly Color LightSquareColor = Color.Gray;
        readonly int consoleHeight, consoleWidth, borderPadding = 4;

        public ConsoleRenderer(Console parent)
        {
            // calculate size for the boardConsole
            consoleHeight = (GlobalConstants.StandardGameTotalBoardRows
                * ConsoleConstants.CharactersPerRowPerBoardSquare) + borderPadding;
            consoleWidth = (GlobalConstants.StandardGameTotalBoardCols
                * ConsoleConstants.CharactersPerColPerBoardSquare) + borderPadding;

            // load font
            var fontMaster = SadConsole.Global.LoadFont("Fonts/C64.font");
            var halfSizedFont = fontMaster.GetFont(SadConsole.Font.FontSizes.Half);

            // create console
            boardConsole = new Console(consoleWidth, consoleHeight, halfSizedFont)
            {
                Parent = parent
            };

            // test
            var board = new Board.Board();
            RenderBoard(board);
        }

        public void RenderMainMenu()
        {
            
        }

        public void RenderBoard(IBoard board)
        {
            int startRowPrint = 0, currentRowPrint = 0, startColPrint = 0, currentColPrint = 0;

            PrintBorder(startRowPrint, startColPrint, board.TotalRows, board.TotalCols);

            int counter = 1;
            for (int top = 0; top < board.TotalRows; top++)
            {
                for (int left = 0; left < board.TotalCols; left++)
                {
                    int offset = borderPadding / 2;
                    currentColPrint = startRowPrint + (left * ConsoleConstants.CharactersPerColPerBoardSquare) + offset;
                    currentRowPrint = startColPrint + (top * ConsoleConstants.CharactersPerRowPerBoardSquare) + offset;

                    Color backgroundColor = (counter % 2 == 0) ? LightSquareColor : DarkSquareColor;

                    // test
                    PrintEmptySquare(backgroundColor, currentRowPrint, currentColPrint);

                    // var position = Position.FromArrayCoordinates(top, left, board.TotalRows);
                    // var figure = board.GetFigureAtPosition(position);
                    // ConsoleHelpers.PrintFigure(figure, backgroundColor, currentRowPrint, currentColPrint);

                    counter++;
                }

                counter++;
            }
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
            int yStart = startRowPrint + (ConsoleConstants.CharactersPerRowPerBoardSquare / 2);
            for (int i = 0; i < boardTotalRows; i++)
            {
                y = yStart + (i * ConsoleConstants.CharactersPerColPerBoardSquare) + borderPadding / 2;

                // left column
                x = startColPrint + 1;
                boardConsole.SetGlyph(x, y, (char)('0' + boardTotalRows - i));

                // right column
                x += boardTotalCols * ConsoleConstants.CharactersPerColPerBoardSquare + 1;
                boardConsole.SetGlyph(x, y, (char)('0' + boardTotalRows - i));
            }
        }

        public void PrintEmptySquare(Color backgroundColor, int top, int left)
        {
            for (int y = 0; y < ConsoleConstants.CharactersPerRowPerBoardSquare; y++)
            {
                boardConsole.Print(left, top + y, 
                    new string(' ', ConsoleConstants.CharactersPerColPerBoardSquare),
                    Color.White, backgroundColor);
            }
        }
    }
}
