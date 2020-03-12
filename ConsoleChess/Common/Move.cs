namespace ConsoleChess.Common
{
    public struct Move
    {
        public Move(Position from, Position to) : this()
        {
            From = from;
            To = to;
        }

        public Position From { get; private set; }

        public Position To { get; private set; }

        public override string ToString()
        {
            return $"{From} - {To}";
        }
    }
}
