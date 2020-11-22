namespace Gtt.Chess.Engine
{
    public class BoardStatus
    {
        public BoardStatus(CellStatus[] cells, Color turn)
        {
            Cells = cells;
            Turn = turn.ToString();
        }

        public string Turn { get; protected set; }

        public CellStatus[] Cells { get; protected set; }
    }
}