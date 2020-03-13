namespace ConsoleChess.Renderers
{
    // .NET
    using System;
    using System.Collections.Generic;

    // Libraries
    using SadConsole;
    using SadConsole.Input;
    using Microsoft.Xna.Framework;
    using Console = SadConsole.Console;

    // Chess
    using Common;
    using Contracts;
    using Figures.Contracts;
    using Board.Contracts;
    using ConsoleChess.Figures;

    public class ConsoleRenderer : IRenderer
    {
        readonly Color DarkSquareColor = Color.DarkGray;
        readonly Color LightSquareColor = Color.Gray;
        readonly Font normalSizedFont;
        int boardTotalRows, boardTotalCols;

        public Console Console { get; private set; }

        public ConsoleRenderer(ContainerConsole parent)
        {
            // load font
            var fontMaster = Global.LoadFont("Fonts/chess.font");
            normalSizedFont = fontMaster.GetFont(Font.FontSizes.One);

            // create board console
            Console = new Console(GlobalConstants.BoardWidth, GlobalConstants.BoardHeight, normalSizedFont)
            {
                Parent = parent
            };
        }

        public void RenderMainMenu()
        {
            
        }

        public void RenderBoard(IBoard board)
        {
            int currentX, currentY;
            boardTotalRows = board.TotalRows;
            boardTotalCols = board.TotalCols;
            PrintBorder();

            int counter = 1;
            for (int y = 0; y < board.TotalRows; y++)
            {
                for (int x = 0; x < board.TotalCols; x++)
                {
                    currentX = x * GlobalConstants.CharactersPerColPerBoardSquare 
                        + GlobalConstants.BorderWidth;
                    currentY = y * GlobalConstants.CharactersPerRowPerBoardSquare 
                        + GlobalConstants.BorderWidth;

                    Color backgroundColor = (counter % 2 == 0) ? LightSquareColor : DarkSquareColor;

                    var position = Position.FromArrayCoordinates(y, x, board.TotalRows);
                    var figure = board.GetFigureAtPosition(position);
                    PrintFigure(Console, figure, backgroundColor, currentX, currentY);

                    counter++;
                }

                counter++;
            }
        }

        public Console ShowPiecePromotion(ChessColor color)
        {
            int possiblePromotions = 4;
            int height = GlobalConstants.CharactersPerRowPerBoardSquare + GlobalConstants.BorderWidth * 2;
            int width = GlobalConstants.CharactersPerColPerBoardSquare * possiblePromotions + GlobalConstants.BorderWidth * 2;
            int x = (Console.Width - width) / 2;
            int y = (Console.Height - height) / 2;
            var console = new Console(width, height, normalSizedFont)
            {
                Position = new Point(x, y),
                Parent = Console,
                DefaultBackground = Color.Black
            };

            // border
            var rect = new Rectangle(0, 0, width, height);
            var cell = new Cell(Color.White, Color.SlateGray, 0);
            console.DrawBox(rect, cell);

            // create selection of figures
            x = y = GlobalConstants.BorderWidth;
            var figures = new List<IFigure>()
            {
                new Rook(color),
                new Knight(color),
                new Bishop(color),
                new Queen(color)
            };

            // print figures
            int counter = 1;
            foreach (var figure in figures)
            {
                Color backgroundColor = (counter++ % 2 == 0) ? Color.LightSlateGray : Color.SlateGray;
                PrintFigure(console, figure, backgroundColor, x, y);
                x += GlobalConstants.CharactersPerColPerBoardSquare;
            }

            return console;
        }

        public void RemoveHighlight(Position position)
        {
            int counter = position.Row + position.Col;
            Color color = (counter % 2 == 0) ? LightSquareColor : DarkSquareColor;
            HighlightPosition(position, color);
        }

        public void HighlightPosition(Position position, Color color)
        {
            int boardPosX = (position.Col - 'a')
                * GlobalConstants.CharactersPerColPerBoardSquare
                + GlobalConstants.BorderWidth;
            int boardPosY = (boardTotalRows - position.Row)
                * GlobalConstants.CharactersPerRowPerBoardSquare
                + GlobalConstants.BorderWidth;

            var rect = new Rectangle(boardPosX, boardPosY, 
                GlobalConstants.CharactersPerColPerBoardSquare, 
                GlobalConstants.CharactersPerRowPerBoardSquare);
            var cell = new Cell(Color.White, color, 0);

            Console.DrawBox(rect, cell);
        }

        public void PrintErrorMessage(string errorMessage)
        {

        }

        void PrintBorder()
        {
            // check if there is space for border
            #pragma warning disable CS0162 // ignore warning
            if (GlobalConstants.BorderWidth <= 0) return;
            #pragma warning restore CS0162

            int x, y;

            // border
            var rect = new Rectangle(0, 0, GlobalConstants.BoardWidth, GlobalConstants.BoardHeight);
            var cell = new Cell(Color.White, DarkSquareColor, 0);
            Console.DrawBox(rect, cell);

            // check if there is space for column and row annotations
            #pragma warning disable CS0162 // ignore warning
            if (GlobalConstants.BorderWidth < 2) return;
            #pragma warning restore CS0162

            // column annotations above and below board
            int xStart = GlobalConstants.CharactersPerColPerBoardSquare / 2;
            for (int i = 0; i < boardTotalCols; i++)
            {
                x = xStart + (i * GlobalConstants.CharactersPerRowPerBoardSquare) 
                    + GlobalConstants.BorderWidth;

                // top row
                y = GlobalConstants.BorderWidth / 2;
                Console.SetGlyph(x, y, (char)('A' + i));

                // bottom row
                y += GlobalConstants.BorderWidth
                    - (GlobalConstants.BorderWidth % 2 == 0 ? 1 : 0)
                    + boardTotalRows * GlobalConstants.CharactersPerRowPerBoardSquare;
                Console.SetGlyph(x, y, (char)('A' + i));
            }

            // row numbers on the left and right of the board
            int yStart = GlobalConstants.CharactersPerRowPerBoardSquare / 2;
            for (int i = 0; i < boardTotalRows; i++)
            {
                y = yStart + (i * GlobalConstants.CharactersPerColPerBoardSquare) 
                    + GlobalConstants.BorderWidth;

                // left column
                x = GlobalConstants.BorderWidth / 2;
                Console.SetGlyph(x, y, (char)('0' + boardTotalRows - i));

                // right column
                x += GlobalConstants.BorderWidth
                    - (GlobalConstants.BorderWidth % 2 == 0 ? 1 : 0)
                    + boardTotalCols * GlobalConstants.CharactersPerColPerBoardSquare;
                Console.SetGlyph(x, y, (char)('0' + boardTotalRows - i));
            }
        }

        void PrintEmptySquare(Color backgroundColor, int currentX, int currentY)
        {
            for (int y = 0; y < GlobalConstants.CharactersPerRowPerBoardSquare; y++)
            {
                char emptySpace = (char) 0;
                Console.Print(currentX, currentY + y, 
                    new string(emptySpace, GlobalConstants.CharactersPerColPerBoardSquare),
                    Color.White, backgroundColor);
            }
        }

        void PrintFigure(Console console, IFigure figure, Color backgroundColor, int currentX, int currentY)
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
                    console.SetGlyph(currentX + x, currentY + y, figure.Pattern[y, x],
                        figure.Color.ToConsoleColor(), backgroundColor);
                }
            }
        }
    }
}
