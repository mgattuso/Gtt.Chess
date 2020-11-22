using System.Collections.Generic;
using System.Linq;

namespace Gtt.Chess.Engine.Pieces
{
    internal class King : Piece
    {
        public King(Board board, Color color, Cell startingCell) : base(board, color, startingCell)
        {
        }

        public override IEnumerable<Cell> PossibleMoves()
        {
            var straight = Board.AllAvailableRankAndFileCells(CurrentCell, 1, 1, 1, 1);
            var diagonal = Board.GetAllAvailableDiagonalCells(CurrentCell, 1, 1, 1, 1);
            return straight.Concat(diagonal);
        }
    }
}
