namespace Gtt.Chess.Engine
{
    public class CellStatus
    {
        public CellStatus(string cellName, int xAxis, int yAxis, string cellColor, string pieceName, string pieceColor)
        {
            CellName = cellName;
            XAxis = xAxis;
            YAxis = yAxis;
            CellColor = cellColor;
            PieceName = pieceName;
            PieceColor = pieceColor;
        }

        public string CellName { get; protected set; }
        public int XAxis { get; protected set; }
        public int YAxis { get; protected set; }
        public string CellColor { get; protected set; }
        public string PieceName { get; set; }
        public string PieceColor { get; set; }
    }
}