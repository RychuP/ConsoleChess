namespace JustChess.Common
{
    public static class GlobalConstants
    {
        const int standardFontHeight = 16;
        const int chessFontHeight = 8;
        public const int BorderWidth = 2;

        public const int BoardHeight = StandardGameTotalBoardRows
            * CharactersPerRowPerBoardSquare + BorderWidth * 2;

        public const int BoardWidth = StandardGameTotalBoardCols
            * CharactersPerColPerBoardSquare + BorderWidth * 2;

        public const int WindowHeight = BoardHeight 
            / (standardFontHeight / chessFontHeight);

        public const int UserInterfaceWidth = 40;

        public const int CharactersPerRowPerBoardSquare = 9;
        public const int CharactersPerColPerBoardSquare = 9;

        public const int StandardGameNumberOfPlayers = 2;
        public const int StandardGameTotalBoardRows = 8;
        public const int StandardGameTotalBoardCols = 8;

        public const int MinimumRowValueOnBoard = 1;
        public const int MaximumRowValueOnBoard = 8;

        public const char MinimumColumnValueOnBoard = 'a';
        public const char MaximumColumnValueOnBoard = 'h';

        public const string EmptyString = "";
    }
}
