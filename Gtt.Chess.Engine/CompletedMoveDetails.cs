using System;
using System.Collections.Generic;

namespace Gtt.Chess.Engine
{
    public class CompletedMoveDetails
    {
        public CompletedMoveDetails(int serialNumber, TimeSpan timeToMove, Cell2 toCell, bool isCheck, bool isCheckMate, IEnumerable<Move2> associatedMoves)
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
        public Cell2 ToCell { get; }
        public IEnumerable<Move2> AssociatedMoves { get; }
        public bool IsCheckMate { get; }
        public bool IsCheck { get; }
    }
}