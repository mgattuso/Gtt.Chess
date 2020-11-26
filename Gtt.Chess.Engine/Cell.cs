using System;

namespace Gtt.Chess.Engine
{
    internal class Cell
    {
        public Cell(int x, int y)
        {
            if (x < 1 || x > 8) throw new ArgumentException("X must be between 1 and 8 (inclusive)");
            if (y < 1 || y > 8) throw new ArgumentException("Y must be between 1 and 8 (inclusive)");
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public Piece CurrentPiece { get; internal set; }

        public string File => Convert.ToChar(64 + X).ToString();

        public string Rank => Y.ToString();

        public string Name => $"{File}{Rank}";

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

        public bool HasOpposingPiece(Piece otherPiece)
        {
            if (CurrentPiece == null) return false;
            return CurrentPiece.Color != otherPiece.Color;
        }
    }
}