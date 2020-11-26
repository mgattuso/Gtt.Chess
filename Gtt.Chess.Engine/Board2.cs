using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtt.Chess.Engine.Extensions;
using Gtt.Chess.Engine.Pieces;

namespace Gtt.Chess.Engine
{
    public class Board2
    {
        public Game2 Game { get; }
        public const int MaxX = 8;
        public const int MaxY = 8;

        public Board2(Game2 game)
        {
            Game = game;
            Cells = CreateCells();
        }

        private List<Cell2> CreateCells()
        {
            var l = new List<Cell2>();
            for (int y = 1; y <= MaxY; y++)
                for (int x = 1; x <= MaxX; x++)
                    l.Add(new Cell2(this, x, y, l.Count+1));
            return l;
        }

        public IEnumerable<Cell2> Cells { get; }

        public void AddPiece(Piece2 piece, string location)
        {
            Cell2 cell = GetCell(location);
            piece.PlaceOnBoard(cell);
        }

        public Cell2 GetCell(string location)
        {
            return Cells.SingleOrDefault(c => c.Name == location);
        }

        public Cell2 GetCell(int x, int y)
        {
            return Cells.SingleOrDefault(c => c.X == x && c.Y == y);
        }

        public IEnumerable<string> SerializeBoardPositions()
        {
            return Cells.Select(x => x.Reference).ToArray();
        }

        public void RemovePiece(Piece2 piece)
        {
            if (piece == null) throw new ArgumentNullException(nameof(piece));
            Game.Captures[piece.Color.GetOtherColor()].Add(piece);
        }

        public IEnumerable<Cell2> GetAllCellsInRank(Cell2 startingCell)
        {
            return Cells.Where(x => x.Rank == startingCell.Rank);
        }

        public IEnumerable<Cell2> GetAvailableCellsInRank(Cell2 startingCell, int? maxRight = null, int? maxLeft = null, bool cannotTakeCell = false)
        {
            if (maxRight != null && maxRight < 0 || maxRight > MaxY) throw new ArgumentException(nameof(maxRight), $"Must be between 0 and {MaxY}");
            if (maxLeft != null && maxLeft < 0 || maxLeft > MaxY) throw new ArgumentException(nameof(maxLeft), $"Must be between 0 and {MaxY}");

            List<Cell2> results = new List<Cell2>();
            List<Cell2> availableCells = GetAllCellsInRank(startingCell).ToList();

            // Right
            for (int i = 1; i <= (maxRight ?? MaxX); i++)
            {
                var next = availableCells.Move(startingCell, i, 0);
                var r = AnalyzeCellAndStop(startingCell, next, cannotTakeCell);
                if (r.Item1 != null) results.Add(r.Item1);
                if (r.Item2) break;
            }

            // Left
            for (int i = 1; i <= (maxLeft ?? MaxX); i++)
            {
                var next = availableCells.Move(startingCell, -i, 0);
                var r = AnalyzeCellAndStop(startingCell, next, cannotTakeCell);
                if (r.Item1 != null) results.Add(r.Item1);
                if (r.Item2) break;
            }

            return results;
        }

        public IEnumerable<Cell2> GetAllCellsInFile(Cell2 startingCell)
        {
            return Cells.Where(x => x.File == startingCell.File);
        }

        public IEnumerable<Cell2> GetAvailableCellsInFile(Cell2 startingCell, int? maxForward = null, int? maxBackwards = null, bool cannotTakeCell = false)
        {
            if (maxForward != null && maxForward < 0 || maxForward > MaxY) throw new ArgumentException(nameof(maxForward), $"Must be between 0 and {MaxY}");
            if (maxBackwards != null && maxBackwards < 0 || maxBackwards > MaxY) throw new ArgumentException(nameof(maxBackwards), $"Must be between 0 and {MaxY}");

            List<Cell2> results = new List<Cell2>();
            List<Cell2> availableCells = GetAllCellsInFile(startingCell).ToList();

            int side = startingCell.Occupant?.Color == Color.Black ? -1 : 1;

            // positive X
            for (int i = 1; i <= (maxForward ?? MaxY); i++)
            {
                var next = availableCells.Move(startingCell, 0, side * i);
                var r = AnalyzeCellAndStop(startingCell, next, cannotTakeCell);
                if (r.Item1 != null) results.Add(r.Item1);
                if (r.Item2) break;
            }

            for (int i = 1; i <= (maxBackwards ?? MaxY); i++)
            {
                var next = availableCells.Move(startingCell, 0, side * -i);
                var r = AnalyzeCellAndStop(startingCell, next, cannotTakeCell);
                if (r.Item1 != null) results.Add(r.Item1);
                if (r.Item2) break;
            }

            return results;
        }

        public IEnumerable<Cell2> AllRankAndFileCells(Cell2 startingCell)
        {
            return GetAllCellsInRank(startingCell).Union(GetAllCellsInFile(startingCell));
        }

        public IEnumerable<Cell2> AllAvailableRankAndFileCells(Cell2 startingCell, int? maxRight = null, int? maxLeft = null, int? maxForward = null, int? maxBackwards = null, bool cannotTakeCell = false)
        {
            return GetAvailableCellsInRank(startingCell, maxRight, maxLeft, cannotTakeCell)
                .Union(GetAvailableCellsInFile(startingCell, maxForward, maxBackwards, cannotTakeCell));
        }

        public IEnumerable<Cell2> GetAllCellsOnPositiveDiagonal(Cell2 startingCell)
        {
            List<Cell2> results = new List<Cell2>();
            int[] start = { startingCell.X - MaxX, startingCell.Y - MaxY };
            for (int i = 0; i < 16; i++)
            {
                var cell = GetCell(start[0] + i, start[1] + i);
                if (cell != null)
                {
                    results.Add(cell);
                }
            }

            return results;
        }

        public IEnumerable<Cell2> GetAvailableCellsOnPositiveDiagonal(Cell2 startingCell, int? maxForwardRight = null, int? maxBackLeft = null, bool cannotTakeCell = false)
        {
            List<Cell2> results = new List<Cell2>();
            int side = startingCell.Occupant?.Color == Color.Black ? -1 : 1;
            List<Cell2> availableCells = GetAllCellsOnPositiveDiagonal(startingCell).ToList();

            // positive X
            for (int i = 1; i <= (maxForwardRight ?? MaxX); i++)
            {
                var next = availableCells.Move(startingCell, side * i, side * i);
                var r = AnalyzeCellAndStop(startingCell, next, cannotTakeCell);
                if (r.Item1 != null) results.Add(r.Item1);
                if (r.Item2) break;
            }

            for (int i = 1; i <= (maxBackLeft ?? MaxX); i++)
            {
                var next = availableCells.Move(startingCell, side * -i, side * -i);
                if (next == null) break;
                var r = AnalyzeCellAndStop(startingCell, next, cannotTakeCell);
                if (r.Item1 != null) results.Add(r.Item1);
                if (r.Item2) break;
            }

            return results;
        }

        public IEnumerable<Cell2> GetAllCellsOnNegativeDiagonal(Cell2 startingCell)
        {
            List<Cell2> results = new List<Cell2>();
            int[] start = { startingCell.X + MaxX, startingCell.Y - MaxY };
            for (int i = 0; i < 16; i++)
            {
                var cell = GetCell(start[0] - i, start[1] + i);
                if (cell != null)
                {
                    results.Add(cell);
                }
            }

            return results;
        }

        public IEnumerable<Cell2> GetAvailableCellsOnNegativeDiagonal(Cell2 startingCell, int? maxForwardLeft = null, int? maxBackRight = null, bool cannotTakeCell = false)
        {
            List<Cell2> results = new List<Cell2>();
            List<Cell2> availableCells = GetAllCellsOnNegativeDiagonal(startingCell).ToList();
            int side = startingCell.Occupant?.Color == Color.Black ? -1 : 1;

            // positive X
            for (int i = 1; i <= (maxForwardLeft ?? MaxX); i++)
            {
                var next = availableCells.Move(startingCell, side * -i, side * i);
                var r = AnalyzeCellAndStop(startingCell, next, cannotTakeCell);
                if (r.Item1 != null) results.Add(r.Item1);
                if (r.Item2) break;
            }

            for (int i = 1; i <= (maxBackRight ?? MaxX); i++)
            {
                var next = availableCells.Move(startingCell, side * i, side * -i);
                var r = AnalyzeCellAndStop(startingCell, next, cannotTakeCell);
                if (r.Item1 != null) results.Add(r.Item1);
                if (r.Item2) break;
            }

            return results;
        }

        public IEnumerable<Cell2> GetAllDiagonalCells(Cell2 startingCell)
        {
            return GetAllCellsOnPositiveDiagonal(startingCell).Union(GetAllCellsOnNegativeDiagonal(startingCell));
        }

        public IEnumerable<Cell2> GetAllAvailableDiagonalCells(Cell2 startingCell,
            int? maxForwardRight = null, int? maxBackLeft = null, int? maxForwardLeft = null, int? maxBackRight = null, bool cannotTakeCell = false)
        {
            return GetAvailableCellsOnPositiveDiagonal(startingCell, maxForwardRight, maxBackLeft, cannotTakeCell)
                .Union(GetAvailableCellsOnNegativeDiagonal(startingCell, maxForwardLeft, maxBackRight, cannotTakeCell));
        }


        private Tuple<Cell2, bool> AnalyzeCellAndStop(Cell2 currentCell, Cell2 next, bool cannotTakeCell)
        {
            if (Equals(currentCell, next)) return Tuple.Create((Cell2)null, false);
            var currentPiece = currentCell.Occupant;
            if (next == null) return Tuple.Create((Cell2)null, true);

            var targetPiece = next.Occupant;
            if (targetPiece == null)
            {
                // IF CELL IF UNOCCUPIED THEN ADD TO AVAILABLE AND CONTINUE
                return Tuple.Create(next, false);
            }

            // IF CURRENT PIECE IS EMPTY THEN CONTINUE
            if (currentPiece == null) return Tuple.Create(next, false);

            if (!cannotTakeCell && targetPiece.Color != currentPiece.Color)
            {
                // IF OCCUPIED CELL CAN BE TAKEN AND IS A DIFFERENT COLOR THEN ADD TO AVAILABLE
                return Tuple.Create(next, true);
            }

            return Tuple.Create((Cell2)null, true);
        }

        public Cell2 GetRelativeCell(Cell2 startingCell, int x, int y)
        {
            return Cells.Move(startingCell, x, y);
        }
    }
}
