namespace Gtt.Chess.Engine
{
    internal class PieceMoveResult
    {
        public PieceMoveResult(Piece piece, Cell startingCell, Cell endingCell)
        {
            Piece = piece;
            StartingCell = startingCell;
            EndingCell = endingCell;
        }

        public PieceMoveResult(Piece piece, Cell startingCell, Cell endingCell, Piece pieceWon) : this(piece, startingCell, endingCell)
        {
            PieceWon = pieceWon;
        }

        public Piece Piece { get; }
        public Cell EndingCell { get; }
        public Cell StartingCell { get; }
        public Piece PieceWon { get; }
    }
}
