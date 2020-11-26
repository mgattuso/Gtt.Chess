using System;
using Gtt.Chess.Engine.Pieces;

namespace Gtt.Chess.Engine
{
    public class Cell
    {
        public Cell(Board board, int x, int y, int id)
        {
            if (x < 1 || x > Engine.Board.MaxX) throw new ArgumentException($"X must be between 1 and {Engine.Board.MaxX} (inclusive)");
            if (y < 1 || y > Engine.Board.MaxY) throw new ArgumentException($"Y must be between 1 and {Engine.Board.MaxY} (inclusive)");
            Board = board;
            X = x;
            Y = y;
            Id = id;
        }

        public Board Board { get; }
        public int X { get; }
        public int Y { get; }
        public int Id { get; }

        public Piece Occupant { get; protected set; }

        public string File => Convert.ToChar(64 + X).ToString();
        public string Rank => Y.ToString();
        public string Name => $"{File}{Rank}";
        public string Reference => $"{Name}{Occupant?.Reference}";

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Cell other)) return false;
            return Equals(other);
        }

        protected bool Equals(Cell other)
        {
            if (other == null) return false;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public Color Color => (X + Y) % 2 == 1 ? Color.White : Color.Black;

        public bool IsOccupied()
        {
            return Occupant != null;
        }

        public CellUpdate SetPiece(Piece incomingOccupant)
        {
            if (IsOccupied())
            {
                if (Occupant.Color == incomingOccupant.Color)
                {
                    throw new Exception("Piece cannot be set if the current occupant is of the same color");
                }

                Board.RemovePiece(Occupant);
                Occupant = incomingOccupant;
                return new CellUpdate(this,incomingOccupant, Occupant);
            }
            Occupant = incomingOccupant;
            return new CellUpdate(this, incomingOccupant);
        }

        public bool HasOpposingPiece(Piece oppositeTo)
        {
            if (oppositeTo == null) return false;
            return IsOccupied() && Occupant.Color != oppositeTo.Color;
        }

        public void RemovePiece()
        {
            Occupant = null;
        }
    }
}