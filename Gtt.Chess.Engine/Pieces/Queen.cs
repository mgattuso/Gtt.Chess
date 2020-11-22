using System.Collections.Generic;
using System.Linq;

namespace Gtt.Chess.Engine.Pieces
{
    internal class Queen : Piece
    {
        public Queen(Board board, Color color, Cell startingCell) : base(board, color, startingCell)
        {
        }

        public override IEnumerable<Cell> PossibleMoves()
        {
            var straight = Board.AllAvailableRankAndFileCells(CurrentCell);
            var diagonal = Board.GetAllAvailableDiagonalCells(CurrentCell);
            return straight.Concat(diagonal);
        }
    }
}
