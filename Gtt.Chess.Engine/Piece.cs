using System;
using System.Collections.Generic;
using System.Linq;
using Gtt.Chess.Engine.Extensions;

namespace Gtt.Chess.Engine
{
    public abstract class Piece
    {
        protected Piece(Color color, string code, bool hasMoved)
        {
            Color = color;
            Code = code;
            HasMoved = hasMoved;
        }

        public void PlaceOnBoard(Cell cell)
        {
            StartingCell = cell;
            CurrentCell = cell;
            cell.SetPiece(this);
        }

        public CellUpdate MoveTo(Cell cell)
        {
            var currentCell = this.CurrentCell;
            var r = cell.SetPiece(this);
            currentCell.RemovePiece();
            CurrentCell = cell;
            HasMoved = true;
            return r;
        }

        public bool HasMoved { get; protected set; }

        public Cell StartingCell { get; protected set; }

        public Cell CurrentCell { get; protected set; }
        public bool IsOnTheBoard => CurrentCell != null;
        public Color Color { get; }
        public string Name => GetType().Name;
        public string Code { get; }

        public string Reference
        {
            get
            {
                var code = !HasMoved ? Code.ToLowerInvariant() :  Code;
                return $"{Color.ToReference()}{code}";
            }
        }

        public bool IsLegalMoveTo(Cell cell)
        {
            if (cell == null) throw new ArgumentNullException(nameof(cell));
            return PossibleMoves().Contains(cell);
        }

        public abstract IEnumerable<Cell> PossibleMoves();
    }
}