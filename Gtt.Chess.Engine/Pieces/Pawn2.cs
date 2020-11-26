using System;
using System.Collections.Generic;
using System.Text;

namespace Gtt.Chess.Engine.Pieces
{
    public class Pawn2 : Piece2
    {
        public Pawn2(Color color, bool hasMoved) : base(color, "P", hasMoved)
        {
        }

        public override IEnumerable<Cell2> PossibleMoves()
        {
            // RULE #1: PAWN CAN MOVE 2 SQUARES ON FIRST MOVE
            List<Cell2> legalCells = new List<Cell2>();
            int maxForward = HasMoved ? 1 : 2;
            var cells = CurrentCell.Board.GetAvailableCellsInFile(CurrentCell, maxForward, 0, cannotTakeCell: true);
            legalCells.AddRange(cells);

            // RULE #2: PAWN CAN TAKE ON THE DIAGONAL
            var diagonals = CurrentCell.Board.GetAllAvailableDiagonalCells(CurrentCell, 1, 0, 1, 0);
            foreach (var d in diagonals)
            {
                if (d.HasOpposingPiece(this))
                {
                    legalCells.Add(d);
                }
            }

            return legalCells;
        }
    }
}
