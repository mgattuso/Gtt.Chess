using System.Collections.Generic;

namespace Gtt.Chess.Engine.Pieces
{
    public class Bishop2 : Piece2
    {
        public Bishop2(Color color, bool hasMoved) : base(color, "B", hasMoved)
        {
        }

        public override IEnumerable<Cell2> PossibleMoves()
        {
            // CAN MOVE DIAGONALLY
            List<Cell2> results = new List<Cell2>();
            var diagonals = CurrentCell.Board.GetAllAvailableDiagonalCells(CurrentCell);
            results.AddRange(diagonals);
            return results;
        }
    }
}