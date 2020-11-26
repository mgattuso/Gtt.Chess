using System;
using System.Collections.Generic;

namespace Gtt.Chess.Engine
{
    public class CompletedMoveDetails
    {
        public CompletedMoveDetails(int serialNumber, TimeSpan timeToMove, Cell toCell, bool isCheck, bool isCheckMate, IEnumerable<Move> associatedMoves)
        {
            SerialNumber = serialNumber;
            TimeToMove = timeToMove;
            ToCell = toCell;
            IsCheck = isCheck;
            IsCheckMate = isCheckMate;
            AssociatedMoves = associatedMoves;
        }

        public TimeSpan TimeToMove { get; }
        public int SerialNumber { get; }
        public Cell ToCell { get; }
        public IEnumerable<Move> AssociatedMoves { get; }
        public bool IsCheckMate { get; }
        public bool IsCheck { get; }
    }
}