using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Gtt.Chess.Tests")]

namespace Gtt.Chess.Engine.Extensions
{
    internal static class CellsExtensions
    {
        public static string PrintCells(this IEnumerable<Cell> cells)
        {
            return string.Join(", ", cells.Select(x => x.Name));
        }

        public static Cell Move(this IEnumerable<Cell> cells, Cell startingCell, int x, int y)
        {
            var localCells = cells as Cell[] ?? cells.ToArray();
            if (!localCells.Contains(startingCell)) throw new ArgumentException("Starting cell is not in cell range");
            var endCell = localCells.FirstOrDefault(c => c.X == startingCell.X + x && c.Y == startingCell.Y + y);
            return endCell;
        }
    }
}
