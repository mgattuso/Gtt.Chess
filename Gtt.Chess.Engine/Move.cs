using System;
using System.Collections.Generic;

namespace Gtt.Chess.Engine
{
    public class Move
    {
        protected Move(Piece piece, Cell fromCell, string message)
        {
            Piece = piece;
            FromCell = fromCell;
            Message = message;
            ValidMove = false;
            MoveRecorded = DateTimeOffset.UtcNow;
        }

        protected Move(
            Piece piece,
            Cell fromCell,
            Cell toCell,
            int serialNumber,
            TimeSpan timeToMove,
            bool isCheck,
            bool isCheckMate,
            IEnumerable<Move> associatedMoves) : this(piece, fromCell, "")
        {
            MoveDetails = new CompletedMoveDetails(serialNumber, timeToMove, toCell, isCheck, isCheckMate, associatedMoves);
            ValidMove = true;
        }

        public bool ValidMove { get; }
        public Piece Piece { get; }
        public Cell FromCell { get; }
        public string Message { get; }

        public DateTimeOffset MoveRecorded { get; }

        public CompletedMoveDetails MoveDetails { get; protected set; }

        public static Move InvalidPlay(Piece piece, Cell fromCell, string message)
        {
            return new Move(piece, fromCell, message);
        }
        public static Move ValidPlay(
            Piece piece,
            Cell fromCell,
            Cell toCell,
            int serialNumber,
            TimeSpan duration,
            bool isCheck,
            bool isCheckMate,
            IEnumerable<Move> associatedMoves)
        {
            return new Move(piece, fromCell, toCell, serialNumber, duration, isCheck, isCheckMate, associatedMoves);
        }
    }
}