using System.Collections.Generic;

namespace Gtt.Chess.Engine.Pieces
{
    internal class Bishop : Piece
    {
        public Bishop(Board board, Color color, Cell startingCell) : base(board, color, startingCell)
        {
        }

        public override IEnumerable<Cell> PossibleMoves()
        {
            // CAN MOVE DIAGONALLY
            List<Cell> results = new List<Cell>();
            var diagonals = Board.GetAllAvailableDiagonalCells(CurrentCell);
            results.AddRange(diagonals);
            return results;
        }
    }
}
