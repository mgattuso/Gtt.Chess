using System;
using System.Collections.Generic;

namespace Gtt.Chess.Engine
{
    public class Move2
    {
        protected Move2(Piece2 piece, Cell2 fromCell, string message)
        {
            Piece = piece;
            FromCell = fromCell;
            Message = message;
            ValidMove = false;
            MoveRecorded = DateTimeOffset.UtcNow;
        }

        protected Move2(
            Piece2 piece,
            Cell2 fromCell,
            Cell2 toCell,
            int serialNumber,
            TimeSpan timeToMove,
            bool isCheck,
            bool isCheckMate,
            IEnumerable<Move2> associatedMoves) : this(piece, fromCell, "")
        {
            MoveDetails = new CompletedMoveDetails(serialNumber, timeToMove, toCell, isCheck, isCheckMate, associatedMoves);
            ValidMove = true;
        }

        public bool ValidMove { get; }
        public Piece2 Piece { get; }
        public Cell2 FromCell { get; }
        public string Message { get; }

        public DateTimeOffset MoveRecorded { get; }

        public CompletedMoveDetails MoveDetails { get; protected set; }

        public static Move2 InvalidPlay(Piece2 piece, Cell2 fromCell, string message)
        {
            return new Move2(piece, fromCell, message);
        }
        public static Move2 ValidPlay(
            Piece2 piece,
            Cell2 fromCell,
            Cell2 toCell,
            int serialNumber,
            TimeSpan duration,
            bool isCheck,
            bool isCheckMate,
            IEnumerable<Move2> associatedMoves)
        {
            return new Move2(piece, fromCell, toCell, serialNumber, duration, isCheck, isCheckMate, associatedMoves);
        }
    }
}