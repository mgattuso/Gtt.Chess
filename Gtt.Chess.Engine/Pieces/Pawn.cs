using System.Collections.Generic;

namespace Gtt.Chess.Engine.Pieces
{
    internal class Pawn : Piece
    {
        public Pawn(Board board, Color color, Cell startingCell) : base(board, color, startingCell)
        {

        }

        public override IEnumerable<Cell> PossibleMoves()
        {
            // RULE #1: PAWN CAN MOVE 2 SQUARES ON FIRST MOVE
            List<Cell> legalCells = new List<Cell>();
            int maxForward = MoveHistory.Count == 0 ? 2 : 1;
            var cells = Board.GetAvailableCellsInFile(CurrentCell, maxForward, 0, cannotTakeCell: true);
            legalCells.AddRange(cells);

            // RULE #2: PAWN CAN TAKE ON THE DIAGONAL
            var diagonals = Board.GetAllAvailableDiagonalCells(CurrentCell, 1, 0, 1, 0);
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
