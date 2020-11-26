namespace Gtt.Chess.Engine
{
    public class CellUpdate
    {
        public Cell Cell { get; }
        public Piece IncomingPiece { get; }
        public Piece DepartingPiece { get; }

        public CellUpdate(Cell cell, Piece incomingPiece)
        {
            Cell = cell;
            IncomingPiece = incomingPiece;
        }

        public CellUpdate(Cell cell, Piece incomingPiece, Piece departingPiece)
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