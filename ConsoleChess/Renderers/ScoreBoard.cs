namespace ConsoleChess.Renderers
{
    using System;
    using System.Collections.Generic;

    using SadConsole;
    using Microsoft.Xna.Framework;
    using Console = SadConsole.Console;

    using Renderers.Contracts;
    using Players.Contracts;
    using Figures.Contracts;
    using Common;

    class ScoreBoard : IScoreBoard
    {
        const int headerHeight = 3;
        const int headerBottomMargin = 1;

        readonly ContainerConsole scoreBoardContainer;
        readonly Dictionary<IPlayer, IList<Move>> moveLists;
        readonly Dictionary<IPlayer, Console> scoreConsoles;

        Color active = Color.White;
        Color passive = Color.DarkGray;

        public ScoreBoard(ContainerConsole parent)
        {
            moveLists = new Dictionary<IPlayer, IList<Move>>();
            scoreConsoles = new Dictionary<IPlayer, Console>();
            scoreBoardContainer = new ContainerConsole()
            {
                Position = new Point(GlobalConstants.BoardWidth, 0),
                Parent = parent
            };
        }

        public void Initialize(IList<IPlayer> players)
        {
            // create individual consoles for each player
            int scoreConsoleWidth = GlobalConstants.UserInterfaceWidth / players.Count;
            int positionX = 0;
            foreach (var player in players)
            {
                // create a score console
                var console = new Console(scoreConsoleWidth, GlobalConstants.WindowHeight)
                {
                    Position = new Point(positionX, 0),
                    Parent = scoreBoardContainer
                };
                scoreConsoles.Add(player, console);

                // create a list of moves
                var moves = new List<Move>();
                moveLists.Add(player, moves);

                // calculate position for the next score console
                positionX += scoreConsoleWidth;

                // print player names
                var currentlyActive = (player.Color == ChessColor.White) ? true : false;
                PrintMoveList(player, currentlyActive);
            }
        }

        public void RecordMove(Move move, IPlayer currentPlayer, IPlayer nextPlayer)
        {
            // record move
            moveLists[currentPlayer].Add(move);

            // redraw
            PrintMoveList(currentPlayer);
            PrintMoveList(nextPlayer, true);
        }

        public void RecordMove(IPlayer currentPlayer, Position? position = null)
        {
            var console = scoreConsoles[currentPlayer];
            var moves = moveLists[currentPlayer];
            int row = headerHeight + headerBottomMargin + moves.Count;
            string move = (position.HasValue) ? $"{position.Value}" : "..";
            console.Print(0, row, $"{move} - ..".Align(HorizontalAlignment.Center, console.Width));
        }

        void PrintMoveList(IPlayer player, bool currentlyActive = false)
        {
            Color fontColor = currentlyActive ? active : passive;
            int row = headerHeight + headerBottomMargin;
            var console = scoreConsoles[player];
            var moves = moveLists[player];
            bool displayTrophiesOnLeftSide = player.Color == ChessColor.Black;

            // display player name at the top
            PrintPlayerName(console, player.Name, fontColor, currentlyActive);

            // display the column of all recorded moves
            for (int i = 0; i < moves.Count; i++)
            {
                string move = moves[i].ToString();
                console.Print(0, row + i, move.Align(HorizontalAlignment.Center, console.Width), fontColor);
            }

            // display a placeholder for the current player's next move
            if (currentlyActive)
            {
                RecordMove(player);
            }

            // display all captured figures
            PrintPlayerTropies(console, player.Trophies, fontColor, row, displayTrophiesOnLeftSide);
        }

        void PrintPlayerTropies(Console console, IList<IFigure> trophies, Color fontColor, int row, bool displayTrophiesOnLeftSide)
        {
            int x = displayTrophiesOnLeftSide ? 1 : console.Width - 2;
            foreach (var figure in trophies)
            {
                string type = figure.GetType().ToString();
                char figureLetter = type.Substring(type.LastIndexOf('.') + 1)[0];
                console.SetGlyph(x, row++, figureLetter, fontColor);
            }
        }

        void PrintPlayerName(Console console, string name, Color borderColor, bool doubleBorder)
        {
            int width = console.Width;
            byte topLeftCorner = 218,
                topRightCorner = 191,
                bottomLeftCorner = 192,
                bottomRightCorner = 217,
                horizontalBorder = 196,
                verticalBorder = 179;
            byte doubleTopLeftCorner = 201,
                doubleTopRightCorner = 187,
                doubleBottomLeftCorner = 200,
                doubleBottomRightCorner = 188,
                doubleHorizontalBorder = 205,
                doubleVerticalBorder = 186;
            char glyph;

            for (int rowIndex = 0; rowIndex < headerHeight; rowIndex++)
            {
                for (int colIndex = 0; colIndex < width; colIndex++)
                {
                    glyph = ' ';

                    if (rowIndex == 0)
                    {
                        if (colIndex == 0)
                        {
                            glyph = (char) (doubleBorder ? doubleTopLeftCorner : topLeftCorner);
                        }
                        else if (colIndex == width - 1)
                        {
                            glyph = (char) (doubleBorder ? doubleTopRightCorner : topRightCorner);
                        }
                        else
                        {
                            glyph = (char) (doubleBorder ? doubleHorizontalBorder : horizontalBorder);
                        }
                    }
                    else if (rowIndex == headerHeight - 1)
                    {
                        if (colIndex == 0)
                        {
                            glyph = (char) (doubleBorder ? doubleBottomLeftCorner : bottomLeftCorner);
                        }
                        else if (colIndex == width - 1)
                        {
                            glyph = (char) (doubleBorder ? doubleBottomRightCorner : bottomRightCorner);
                        }
                        else
                        {
                            glyph = (char) (doubleBorder ? doubleHorizontalBorder : horizontalBorder);
                        }
                    }
                    else if (colIndex == 0 || colIndex == width - 1)
                    {
                        glyph = (char)  (doubleBorder ? doubleVerticalBorder : verticalBorder);
                    }

                    console.SetGlyph(colIndex, rowIndex, glyph, borderColor);
                }
            }
            console.Print(1, headerHeight / 3, name.Align(HorizontalAlignment.Center, width - 2));
        }
    }
}
