namespace ConsoleChess.Renderers
{
    using System;
    using System.Collections.Generic;

    using SadConsole;
    using Microsoft.Xna.Framework;
    using Console = SadConsole.Console;

    using Renderers.Contracts;
    using Players.Contracts;
    using Common;

    class ScoreBoard : IScoreBoard
    {
        const int headerHeight = 3;
        const int headerBottomMargin = 1;

        readonly ContainerConsole scoreBoardContainer;
        Dictionary<IPlayer, IList<Move>> moveLists;
        Dictionary<IPlayer, Console> scoreConsoles;

        Color active = Color.Silver;
        Color passive = Color.SlateGray;

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

            PrintPlayerName(console, player.Name, currentlyActive);

            for (int i = 0; i < moves.Count; i++)
            {
                string move = moves[i].ToString();
                console.Print(0, row++, move.Align(HorizontalAlignment.Center, console.Width), fontColor);
            }

            if (currentlyActive)
            {
                RecordMove(player);
            }
        }

        void PrintPlayerName(Console console, string name, bool doubleBorder = false)
        {
            int width = console.Width;
            Color borderColor = doubleBorder ? active : passive;
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
