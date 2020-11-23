using System;
using System.Linq;

namespace Gtt.Chess.Engine
{
    public class GameMoveResult
    {
        public GameMoveResult(PieceMoveResultStatus[] piecesMoved, DateTimeOffset duration, Color[] colorsInCheck, Color? colorWon)
        {
            PiecesMoved = piecesMoved;
            MoveDuration = duration;
            ColorsInCheck = colorsInCheck.Select(x => x.ToString()).ToArray();
            ColorWon = colorWon.ToString();
        }
        public string[] ColorsInCheck { get; }
        public string ColorWon { get; }
        internal PieceMoveResultStatus[] PiecesMoved { get; }
        public DateTimeOffset MoveDuration { get; }
    }
}