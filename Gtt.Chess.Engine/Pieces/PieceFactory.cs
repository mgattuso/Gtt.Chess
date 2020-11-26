using System;
using System.Collections.Generic;
using System.Text;

namespace Gtt.Chess.Engine.Pieces
{
    public static class PieceFactory
    {
        public static Piece2 FromCode(string code, Color color)
        {
            switch (code)
            {
                case "B":
                case "b":
                    return new Bishop2(color, char.IsUpper(code[0]));
                case "K":
                case "k":
                    return new King2(color, char.IsUpper(code[0]));
                case "P":
                case "p":
                    return new Pawn2(color, char.IsUpper(code[0]));
                case "N":
                case "n":
                    return new Knight2(color, char.IsUpper(code[0]));
                case "R":
                case "r":
                    return new Rook2(color, char.IsUpper(code[0]));
                case "Q":
                case "q":
                    return new Queen2(color, char.IsUpper(code[0]));
                default:
                    throw new ArgumentException("Code is not valid", nameof(code));
            }
        }
    }
}
