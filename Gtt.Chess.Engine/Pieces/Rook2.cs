using System.Collections.Generic;

namespace Gtt.Chess.Engine.Pieces
{
    public class Rook2 : Piece2
    {
        public Rook2(Color color, bool hasMoved) : base(color, "R", hasMoved)
        {
        }

        public override IEnumerable<Cell2> PossibleMoves()
        {
            var rankFile = CurrentCell.Board.AllAvailableRankAndFileCells(CurrentCell);
            return rankFile;
        }
    }
}