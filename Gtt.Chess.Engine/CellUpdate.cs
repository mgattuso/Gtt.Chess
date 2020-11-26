namespace Gtt.Chess.Engine
{
    public class CellUpdate
    {
        public Cell2 Cell { get; }
        public Piece2 IncomingPiece { get; }
        public Piece2 DepartingPiece { get; }

        public CellUpdate(Cell2 cell, Piece2 incomingPiece)
        {
            Cell = cell;
            IncomingPiece = incomingPiece;
        }

        public CellUpdate(Cell2 cell, Piece2 incomingPiece, Piece2 departingPiece)
        {
            Cell = cell;
            IncomingPiece = incomingPiece;
            DepartingPiece = departingPiece;
        }

        public bool WasPieceRemoved()
        {
            return DepartingPiece != null;
        }
    }
}