using System;
using System.Linq;

namespace Gtt.Chess.Engine
{
    public class GameMoveResult
    {
        public GameMoveResult(PieceMoveResultStatus[] piecesMoved, DateTimeOffset duration, Color? colorInCheck, Color? colorWon)
        {
            PiecesMoved = piecesMoved;
            MoveDuration = duration;
            ColorInCheck = colorInCheck?.ToString();
            ColorWon = colorWon.ToString();
        }
        public string ColorInCheck { get; }
        public string ColorWon { get; }
        internal PieceMoveResultStatus[] PiecesMoved { get; }
        public DateTimeOffset MoveDuration { get; }
    }
}