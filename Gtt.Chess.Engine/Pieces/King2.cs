using System.Collections.Generic;
using System.Linq;

namespace Gtt.Chess.Engine.Pieces
{
    public class King2 : Piece2
    {
        public King2(Color color, bool hasMoved) : base(color, "K", hasMoved)
        {
        }

        public override IEnumerable<Cell2> PossibleMoves()
        {
            var straight = CurrentCell.Board.AllAvailableRankAndFileCells(CurrentCell, 1, 1, 1, 1);
            var diagonal = CurrentCell.Board.GetAllAvailableDiagonalCells(CurrentCell, 1, 1, 1, 1);
            return straight.Concat(diagonal);
        }
    }
}