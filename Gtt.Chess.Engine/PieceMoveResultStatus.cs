namespace Gtt.Chess.Engine
{
    public class PieceMoveResultStatus
    {
        public PieceMoveResultStatus(string pieceMovedName, string pieceMovedColor, string startingCell, string endingCell, string pieceWonName, string pieceWonColor)
        {
            PieceMovedName = pieceMovedName;
            PieceMovedColor = pieceMovedColor;
            StartingCell = startingCell;
            EndingCell = endingCell;
            PieceWonName = pieceWonName;
            PieceWonColor = pieceWonColor;
        }

        public string PieceMovedName { get; protected set; }
        public string PieceMovedColor { get; protected set; }
        public string StartingCell { get; protected set; }
        public string EndingCell { get; protected set; }

        public string PieceWonName { get; protected set; }
        public string PieceWonColor { get; protected set; }
    }
}