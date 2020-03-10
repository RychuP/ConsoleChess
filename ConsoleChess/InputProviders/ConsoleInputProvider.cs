namespace ConsoleChess.InputProviders
{
    using System;
    using System.Collections.Generic;

    using Common;
    using Contracts;
    using Players;
    using Players.Contracts;

    public class ConsoleInputProvider : IInputProvider
    {
        public IList<IPlayer> GetPlayers(int numberOfPlayers)
        {
            var players = new List<IPlayer>();
            return players;
        }

        public Move GetNextPlayerMove(IPlayer player)
        {

            // temp hack
            return CreateMoveFromCommand("a5-c5");
        }

        Move CreateMoveFromCommand(string command)
        {
            var positionAsStringParts = command.Split('-');
            if (positionAsStringParts.Length != 2)
            {
                throw new InvalidOperationException("Invalid command!");
            }

            var fromAsString = positionAsStringParts[0];
            var toAsString = positionAsStringParts[1];

            var fromPosition = Position.FromChessCoordinates(fromAsString[1] - '0', fromAsString[0]);
            var toPosition = Position.FromChessCoordinates(toAsString[1] - '0', toAsString[0]);

            return new Move(fromPosition, toPosition);
        }
    }
}
