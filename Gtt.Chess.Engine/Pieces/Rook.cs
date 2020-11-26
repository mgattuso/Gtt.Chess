using System.Collections.Generic;

namespace Gtt.Chess.Engine.Pieces
{
    public class Rook : Piece
    {
        public Rook(Color color, bool hasMoved) : base(color, "R", hasMoved)
        {
        }

        public override IEnumerable<Cell> PossibleMoves()
        {
            var rankFile = CurrentCell.Board.AllAvailableRankAndFileCells(CurrentCell);
            return rankFile;
        }
    }
}