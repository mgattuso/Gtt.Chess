using System;

namespace Gtt.Chess.Engine
{
    public class GameMoveResult
    {
        public GameMoveResult(PieceMoveResultStatus[] piecesMoved, DateTimeOffset duration, Color[] colorsInCheck, Color? colorWon)
        {
            PiecesMoved = piecesMoved;
            MoveDuration = duration;
            ColorsInCheck = colorsInCheck;
            ColorWon = colorWon;
        }
        public Color[] ColorsInCheck { get; }
        public Color? ColorWon { get; }
        internal PieceMoveResultStatus[] PiecesMoved { get; }
        public DateTimeOffset MoveDuration { get; }
    }
}