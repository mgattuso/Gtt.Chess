﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Gtt.Chess.Tests")]

namespace Gtt.Chess.Engine.Extensions
{
    internal static class CellsExtensions
    {
        public static Cell2 Move(this IEnumerable<Cell2> cells, Cell2 startingCell, int x, int y)
        {
            var localCells = cells as Cell2[] ?? cells.ToArray();
            if (!localCells.Contains(startingCell)) throw new ArgumentException("Starting cell is not in cell range");
            var endCell = localCells.FirstOrDefault(c => c.X == startingCell.X + x && c.Y == startingCell.Y + y);
            return endCell;
        }
    }
}
