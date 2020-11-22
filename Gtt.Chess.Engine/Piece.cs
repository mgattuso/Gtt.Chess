using System.Collections.Generic;
using System.Linq;

namespace Gtt.Chess.Engine
{
    internal abstract class Piece
    {
        protected Piece(Board board, Color color, Cell startingCell)
        {
            Board = board;
            CurrentCell = startingCell;
            CurrentCell.CurrentPiece = this;
            Color = color;
        }

        public Cell CurrentCell { get; protected set; }

        public bool IsOnTheBoard => CurrentCell != null;

        public Color Color { get; }

        protected Board Board { get; }

        public PieceMoveResult[] MoveTo(Cell cell)
        {
            if (!IsOnTheBoard || cell == null || cell.Equals(CurrentCell))
            {
                return new PieceMoveResult[0];
            }

            var startingCell = CurrentCell;
            var targetPiece = cell.CurrentPiece;
            if (IsLegalMoveTo(cell))
            {
                CurrentCell.CurrentPiece = null;
                CurrentCell = cell;
                cell.CurrentPiece = this;
                MoveHistory.Add(new Cell(cell.X, cell.Y));
                return new[]
                {
                    new PieceMoveResult(this, startingCell, cell, targetPiece)
                };
            }

            return new PieceMoveResult[0];
        }

        public bool IsLegalMoveTo(Cell cell)
        {
            return IsOnTheBoard && PossibleMoves().Contains(cell);
        }


        public abstract IEnumerable<Cell> PossibleMoves();

        public List<Cell> MoveHistory { get; } = new List<Cell>();

        public string Name => this.GetType().Name;

        public IEnumerable<Piece> ThreatenedBy()
        {
            var canTake = Board.Cells
                             .Where(x => x.HasOpposingPiece(this))
                             .Select(x => x.CurrentPiece)
                             .Where(x => x.PossibleMoves().Contains(CurrentCell));

            return canTake;
        }
    }
}
