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
    using Players;

    class ScoreBoard : IScoreBoard
    {
        readonly ContainerConsole scoreBoardContainer;
        Dictionary<IPlayer, Move> moves;
        Dictionary<IPlayer, Console> scoreConsoles;

        public ScoreBoard(ContainerConsole parent)
        {
            moves = new Dictionary<IPlayer, Move>();
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
                var console = new Console(scoreConsoleWidth, GlobalConstants.WindowHeight)
                {
                    Position = new Point(positionX, 0),
                    Parent = scoreBoardContainer
                };
                scoreConsoles.Add(player, console);
                positionX += scoreConsoleWidth;
                console.Print(2, 2, player.Name);
            }
        }

        public void RecordMove(Move move, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
