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
            var basicCheck = IsOnTheBoard && PossibleMoves().Contains(cell);
            if (!basicCheck)
            {
                return false;
            }

            if (!Board.ReplayMode)
            {
                var simulatedWithMove = new Game(GameStyle.Traditional, Board.History, CurrentCell.Name, cell.Name);
                var colorsInCheck = simulatedWithMove.Board.WhatColorIsInCheck();
                return !colorsInCheck.Contains(CurrentCell.CurrentPiece.Color);
            }

            return true;
        }


        public abstract IEnumerable<Cell> PossibleMoves();

        public List<Cell> MoveHistory { get; } = new List<Cell>();

        public string Name => this.GetType().Name;

        public IEnumerable<Piece> ThreatenedBy()
        {
            var canTake = Board.Cells
                             .Where(x => x.CurrentPiece != null)
                             .Where(x => x.HasOpposingPiece(this))
                             .Select(x => x.CurrentPiece)
                             .Where(x => x.PossibleMoves().Contains(CurrentCell)).ToList();

            return canTake;
        }
    }
}
