namespace JustChess.Common
{
    public struct Move
    {
        public Move(Position from, Position to) : this()
        {
            Type = MoveType.Normal;
            FigureLetter = '?';
            From = from;
            To = to;
        }

        public Position From { get; private set; }

        public Position To { get; private set; }

        public MoveType Type { get; set; }

        public char FigureLetter { get; set; }

        public override string ToString()
        {
            string move;
            switch (Type)
            {
                case MoveType.CastleKingSide:
                    move = "O - O";
                    break;

                case MoveType.CastleQueenSide:
                    move = "OO - OO";
                    break;

                case MoveType.Capture:
                    move = $"{FigureLetter}{From} x {To}";
                    break;

                default:
                    move = $"{FigureLetter}{From} - {To}";
                    break;
            }

            return move;
        }
    }
}
