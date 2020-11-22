using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtt.Chess.Engine.Extensions;
using Gtt.Chess.Engine.Pieces;

namespace Gtt.Chess.Engine
{
    internal class Board
    {
        private static int MaxX = 8;
        private static int MaxY = 8;

        public Board()
        {
            Cells = CreateCells();
            CurrentTurn = Color.White;


        }

        internal PieceMoveResult[] Move(string from, string to)
        {
            var cell = GetCell(from);
            if (cell?.CurrentPiece == null)
            {
                return new PieceMoveResult[0];
            }

            if (cell.CurrentPiece.Color != CurrentTurn)
            {
                return new PieceMoveResult[0];
            }

            var target = GetCell(to);
            if (target == null)
            {
                return new PieceMoveResult[0];

            }

            var r = cell.CurrentPiece.MoveTo(target);
            if (r.Any())
            {
                CurrentTurn = CurrentTurn == Color.White ? Color.Black : Color.White;
            }

            return r;
        }

        private static List<Cell> CreateCells()
        {
            var l = new List<Cell>();
            for (int y = 1; y <= MaxY; y++)
            {
                for (int x = 1; x <= MaxX; x++)
                {
                    l.Add(new Cell(x, y));
                }
            }

            return l;
        }

        public List<Cell> Cells { get; set; }
        public Color CurrentTurn { get; protected set; }

        public IEnumerable<Cell> GetAllCellsInRank(Cell startingCell)
        {
            return Cells.Where(x => x.Rank == startingCell.Rank);
        }

        public IEnumerable<Cell> GetAvailableCellsInRank(Cell startingCell, int? maxRight = null, int? maxLeft = null, bool cannotTakeCell = false)
        {
            if (maxRight != null && maxRight < 0 || maxRight > MaxY) throw new ArgumentException(nameof(maxRight), $"Must be between 0 and {MaxY}");
            if (maxLeft != null && maxLeft < 0 || maxLeft > MaxY) throw new ArgumentException(nameof(maxLeft), $"Must be between 0 and {MaxY}");

            List<Cell> results = new List<Cell>();
            List<Cell> availableCells = GetAllCellsInRank(startingCell).ToList();

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

        public IEnumerable<Cell> GetAllCellsInFile(Cell startingCell)
        {
            return Cells.Where(x => x.File == startingCell.File);
        }

        private Tuple<Cell, bool> AnalyzeCellAndStop(Cell currentCell, Cell next, bool cannotTakeCell)
        {
            if (currentCell == next) return Tuple.Create((Cell)null, false);
            var currentPiece = currentCell.CurrentPiece;
            if (next == null) return Tuple.Create((Cell)null, true);

            var targetPiece = next.CurrentPiece;
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

            return Tuple.Create((Cell)null, true);
        }

        public IEnumerable<Cell> GetAvailableCellsInFile(Cell startingCell, int? maxForward = null, int? maxBackwards = null, bool cannotTakeCell = false)
        {
            if (maxForward != null && maxForward < 0 || maxForward > MaxY) throw new ArgumentException(nameof(maxForward), $"Must be between 0 and {MaxY}");
            if (maxBackwards != null && maxBackwards < 0 || maxBackwards > MaxY) throw new ArgumentException(nameof(maxBackwards), $"Must be between 0 and {MaxY}");

            List<Cell> results = new List<Cell>();
            List<Cell> availableCells = GetAllCellsInFile(startingCell).ToList();

            int side = startingCell.CurrentPiece?.Color == Color.Black ? -1 : 1;

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

        public IEnumerable<Cell> AllRankAndFileCells(Cell startingCell)
        {
            return GetAllCellsInRank(startingCell).Union(GetAllCellsInFile(startingCell));
        }

        public IEnumerable<Cell> AllAvailableRankAndFileCells(Cell startingCell, int? maxRight = null, int? maxLeft = null, int? maxForward = null, int? maxBackwards = null, bool cannotTakeCell = false)
        {
            return GetAvailableCellsInRank(startingCell, maxRight, maxLeft, cannotTakeCell)
                    .Union(GetAvailableCellsInFile(startingCell, maxForward, maxBackwards, cannotTakeCell));
        }

        public IEnumerable<Cell> GetAllCellsOnPositiveDiagonal(Cell startingCell)
        {
            List<Cell> results = new List<Cell>();
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

        public IEnumerable<Cell> GetAvailableCellsOnPositiveDiagonal(Cell startingCell, int? maxForwardRight = null, int? maxBackLeft = null, bool cannotTakeCell = false)
        {
            List<Cell> results = new List<Cell>();
            int side = startingCell.CurrentPiece?.Color == Color.Black ? -1 : 1;
            List<Cell> availableCells = GetAllCellsOnPositiveDiagonal(startingCell).ToList();

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

        public IEnumerable<Cell> GetAllCellsOnNegativeDiagonal(Cell startingCell)
        {
            List<Cell> results = new List<Cell>();
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

        public IEnumerable<Cell> GetAvailableCellsOnNegativeDiagonal(Cell startingCell, int? maxForwardLeft = null, int? maxBackRight = null, bool cannotTakeCell = false)
        {
            List<Cell> results = new List<Cell>();
            List<Cell> availableCells = GetAllCellsOnNegativeDiagonal(startingCell).ToList();
            int side = startingCell.CurrentPiece?.Color == Color.Black ? -1 : 1;

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

        public IEnumerable<Cell> GetAllDiagonalCells(Cell startingCell)
        {
            return GetAllCellsOnPositiveDiagonal(startingCell).Union(GetAllCellsOnNegativeDiagonal(startingCell));
        }

        public IEnumerable<Cell> GetAllAvailableDiagonalCells(Cell startingCell,
            int? maxForwardRight = null, int? maxBackLeft = null, int? maxForwardLeft = null, int? maxBackRight = null, bool cannotTakeCell = false)
        {
            return GetAvailableCellsOnPositiveDiagonal(startingCell, maxForwardRight, maxBackLeft, cannotTakeCell)
                .Union(GetAvailableCellsOnNegativeDiagonal(startingCell, maxForwardLeft, maxBackRight, cannotTakeCell));
        }

        private Cell GetCell(int x, int y)
        {
            return Cells.FirstOrDefault(c => c.X == x && c.Y == y);
        }

        public Cell GetRelativeCell(Cell startingCell, int x, int y)
        {
            return Cells.Move(startingCell, x, y);
        }

        public Cell GetCell(string name)
        {
            var r = Cells.FirstOrDefault(x => x.Name == name);
            return r;
        }

        public IEnumerable<Piece> FindPiecesByType<T>() where T : Piece
        {
            return Cells.Where(x => x.CurrentPiece != null).Select(x => x.CurrentPiece).OfType<T>();
        }

        public Color[] WhatColorIsInCheck()
        {
            return new Color[0];
            //    var kings = FindPiecesByType<King>();
            //    var threatened = kings.Where(x => x.ThreatenedBy().Any()).Select(x => x.Color).ToArray();
            //    return threatened;
        }

        public string PrintBoard()
        {
            StringBuilder sb = new StringBuilder();

            string horizontalBorder = "#";
            for (int i = 0; i < 8; i++)
            {
                horizontalBorder = horizontalBorder + "#########";
            }

            sb.AppendLine(horizontalBorder);

            var rows = Cells.GroupBy(x => x.Y).OrderByDescending(x => x.Key);

            foreach (var row in rows)
            {
                string line1 = "#";
                string line2 = "#";
                string line3 = "#";
                string line4 = "#";
                string line5 = "#";
                string line6 = "#";
                string line8 = "#";
                foreach (var cell in row)
                {
                    line1 = line1 + "        #";
                    line2 = line2 + $"   {cell.Name}   #";
                    line3 = line3 + $"        #";
                    line4 = line4 + $"{cell.CurrentPiece?.Color,8}#";
                    line5 = line5 + $"{cell.CurrentPiece?.Name,8}#";
                    line6 = line6 + $"        #";
                    line8 = line8 + $"{cell.Color,8}#";
                }

                sb.AppendLine(line1);
                sb.AppendLine(line2);
                sb.AppendLine(line3);
                sb.AppendLine(line4);
                sb.AppendLine(line5);
                sb.AppendLine(line6);
                sb.AppendLine(line8);
                sb.AppendLine(horizontalBorder);
            }
            return sb.ToString();
        }
    }
}
