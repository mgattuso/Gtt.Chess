using System.Collections.Generic;

namespace Gtt.Chess.Engine.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color, bool hasMoved) : base(color, "B", hasMoved)
        {
        }

        public override IEnumerable<Cell> PossibleMoves()
        {
            // CAN MOVE DIAGONALLY
            List<Cell> results = new List<Cell>();
            var diagonals = CurrentCell.Board.GetAllAvailableDiagonalCells(CurrentCell);
            results.AddRange(diagonals);
            return results;
        }
    }
}