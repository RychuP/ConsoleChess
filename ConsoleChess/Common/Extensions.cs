using System;
using Microsoft.Xna.Framework;

namespace ConsoleChess.Common
{
    public static class Extensions
    {
        public static Color ToConsoleColor(this ChessColor chessColor)
        {
            switch (chessColor)
            {
                case ChessColor.Black:
                    return Color.Black;
                case ChessColor.White:
                    return Color.White;
                case ChessColor.Brown:
                    return Color.Brown;
                default:
                    throw new InvalidOperationException("Cannot convert chess color!");
            }
        }
    }
}
