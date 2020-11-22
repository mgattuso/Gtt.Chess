using System.Collections.Generic;

namespace Gtt.Chess.Engine.Pieces
{
    internal class Rook : Piece
    {
        public Rook(Board board, Color color, Cell startingCell) : base(board, color, startingCell)
        {
        }

        public override IEnumerable<Cell> PossibleMoves()
        {
            var straight = Board.AllAvailableRankAndFileCells(CurrentCell);
            return straight;
        }
    }
}
