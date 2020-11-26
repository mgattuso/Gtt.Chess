using System.Collections.Generic;
using System.Linq;

namespace Gtt.Chess.Engine.Pieces
{
    public class Queen2 : Piece2
    {
        public Queen2(Color color, bool hasMoved) : base(color, "Q", hasMoved)
        {
        }

        public override IEnumerable<Cell2> PossibleMoves()
        {
            var straight = CurrentCell.Board.AllAvailableRankAndFileCells(CurrentCell);
            var diagonal = CurrentCell.Board.GetAllAvailableDiagonalCells(CurrentCell);
            return straight.Concat(diagonal);
        }
    }
}