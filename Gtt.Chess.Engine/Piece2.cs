using System;
using System.Collections.Generic;
using System.Linq;
using Gtt.Chess.Engine.Extensions;

namespace Gtt.Chess.Engine
{
    public abstract class Piece2
    {
        protected Piece2(Color color, string code, bool hasMoved)
        {
            Color = color;
            Code = code;
            HasMoved = hasMoved;
        }

        public void PlaceOnBoard(Cell2 cell)
        {
            StartingCell = cell;
            CurrentCell = cell;
            cell.SetPiece(this);
        }

        public CellUpdate MoveTo(Cell2 cell)
        {
            var currentCell = this.CurrentCell;
            var r = cell.SetPiece(this);
            currentCell.RemovePiece();
            CurrentCell = cell;
            HasMoved = true;
            return r;
        }

        public bool HasMoved { get; protected set; }

        public Cell2 StartingCell { get; protected set; }

        public Cell2 CurrentCell { get; protected set; }
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

        public bool IsLegalMoveTo(Cell2 cell)
        {
            if (cell == null) throw new ArgumentNullException(nameof(cell));
            return PossibleMoves().Contains(cell);
        }

        public abstract IEnumerable<Cell2> PossibleMoves();
    }
}