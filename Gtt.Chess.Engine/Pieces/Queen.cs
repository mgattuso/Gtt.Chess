using System.Collections.Generic;
using System.Linq;

namespace Gtt.Chess.Engine.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color, bool hasMoved) : base(color, "Q", hasMoved)
        {
        }

        public override IEnumerable<Cell> PossibleMoves()
        {
            var straight = CurrentCell.Board.AllAvailableRankAndFileCells(CurrentCell);
            var diagonal = CurrentCell.Board.GetAllAvailableDiagonalCells(CurrentCell);
            return straight.Concat(diagonal);
        }
    }
}