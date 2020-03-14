namespace JustChess.Renderers
{
    // .NET
    using System;
    using System.Collections.Generic;

    // Libraries
    using SadConsole;
    using SadConsole.Controls;
    using Microsoft.Xna.Framework;
    using Console = SadConsole.Console;

    // Chess
    using Common;
    using Contracts;
    using Figures;
    using Figures.Contracts;
    using Board.Contracts;

    public class ConsoleRenderer : IRenderer
    {
        readonly Color darkSquareColor = Color.DarkGray;
        readonly Color lightSquareColor = Color.Gray;
        readonly Color menuBorderColor = Color.SlateGray;
        readonly Font normalSizedFont;
        int boardTotalRows, boardTotalCols, piecePattern = 1;

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

        public void RenderMainMenu(EventHandler newGameHandler, EventHandler changeAppearanceHandler)
        {
            int buttonHeight = 3;
            int spacingBetweenButtons = 2;

            int width = Console.Width / 2;
            int height = Console.Height / 2;
            int x = width / 2;
            int y = height / 2;

            var console = CreateChildConsole(width, height, x, y);
            DrawOutline(console, menuBorderColor);

            var controls = new ControlsConsole(width - GlobalConstants.BorderWidth * 2 , (height / 2) - GlobalConstants.BorderWidth)
            {
                Position = new Point(GlobalConstants.BorderWidth, GlobalConstants.BorderWidth / 2),
                Parent = console
            };

            var newGame = new Button(20, buttonHeight)
            {
                Text = "New Game",
                Position = new Point((controls.Width / 2) - 10, spacingBetweenButtons),
                UseMouse = true,
                UseKeyboard = false,
            };
            newGame.Click += newGameHandler;
            controls.Add(newGame);

            var changeAppearance = new Button(20, buttonHeight)
            {
                Text = "Change Appearance",
                Position = new Point((controls.Width / 2) - 10, spacingBetweenButtons * 2 + buttonHeight),
                UseMouse = true,
                UseKeyboard = false,
            };
            changeAppearance.Click += changeAppearanceHandler;
            controls.Add(changeAppearance);

            var exitGame = new Button(20, buttonHeight)
            {
                Text = "Exit Game",
                Position = new Point((controls.Width / 2) - 10, spacingBetweenButtons * 3 + buttonHeight * 2),
                UseMouse = true,
                UseKeyboard = false,
            };
            exitGame.Click += ButtonPressExitGame;
            controls.Add(exitGame);
        }

        void ButtonPressExitGame(object sender, EventArgs e)
        {
            Environment.Exit(0);
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

                    Color backgroundColor = (counter % 2 == 0) ? lightSquareColor : darkSquareColor;

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
            var console = CreateChildConsole(width, height, x, y);

            DrawOutline(console, menuBorderColor);

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
                Color backgroundColor = (counter++ % 2 == 0) ? Color.LightSlateGray : menuBorderColor;
                PrintFigure(console, figure, backgroundColor, x, y);
                x += GlobalConstants.CharactersPerColPerBoardSquare;
            }

            return console;
        }

        public void RemoveHighlight(Position position)
        {
            int counter = position.Row + position.Col;
            Color color = (counter % 2 == 0) ? lightSquareColor : darkSquareColor;
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

        public void TogglePiecePatterns()
        {
            piecePattern = piecePattern == 0 ? 1 : 0;
        }

        void DrawOutline(Console console, Color color)
        {
            var rect = new Rectangle(0, 0, console.Width, console.Height);
            var cell = new Cell(Color.White, color, 0);
            console.DrawBox(rect, cell);
        }

        Console CreateChildConsole(int width, int height, int x, int y)
        {
            return new Console(width, height, normalSizedFont)
            {
                Position = new Point(x, y),
                Parent = Console,
                DefaultBackground = Color.Black
            };
        }

        void PrintBorder()
        {
            // check if there is space for border
            #pragma warning disable CS0162 // ignore warning
            if (GlobalConstants.BorderWidth <= 0) return;
            #pragma warning restore CS0162

            int x, y;
            DrawOutline(Console, darkSquareColor);

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

            for (int y = 0; y < figure.Pattern[piecePattern].GetLength(0); y++)
            {
                for (int x = 0; x < figure.Pattern[piecePattern].GetLength(1); x++)
                {
                    console.SetGlyph(currentX + x, currentY + y, figure.Pattern[piecePattern][y, x],
                        figure.Color.ToConsoleColor(), backgroundColor);
                }
            }
        }
    }
}
