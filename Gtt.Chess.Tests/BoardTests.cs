using System;
using System.Linq;
using Gtt.Chess.Engine;
using Gtt.Chess.Engine.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtt.Chess.Tests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void BoardIsInitializedWith64Squares()
        {
            var b = new Board();
            Assert.AreEqual(64, b.Cells.Count);
        }

        [TestMethod]
        public void AllRankCells()
        {
            var b = new Board();
            var c = b.Cells[0];
            Assert.AreEqual("A1", c.Name);
            var cells = b.GetAllCellsInRank(c).ToList();
            Assert.AreEqual(8, cells.Count());
            Assert.AreEqual(8, cells.Count(x => x.Rank == "1"));
            Assert.AreEqual(1, cells.Count(x => x.File == "A"));
            Assert.AreEqual(8, cells.Select(x => x.File).Distinct().Count());
        }

        [TestMethod]
        public void AllAvailableRankCells()
        {
            var b = new Board();
            var p1 = new Pawn(b, Color.White, b.GetCell("D2"));
            var p2 = new Pawn(b, Color.White, b.GetCell("F2"));
            var cells = b.GetAvailableCellsInRank(p1.CurrentCell).OrderBy(x => x.Name).ToList();
            //Assert.AreEqual("A2, B2, C2, E2", cells.PrintCells());
        }

        [TestMethod]
        public void GetPawnStartingMove()
        {
            var b = new Board();
            var p1 = new Pawn(b, Color.White, b.GetCell("B2"));
            var p2 = new Pawn(b, Color.Black, b.GetCell("C3"));
            var p3 = new Pawn(b, Color.Black, b.GetCell("A3"));
            //Console.WriteLine(p1.PossibleMoves().PrintCells());
        }

        [TestMethod]
        public void AllFileCells()
        {
            var b = new Board();
            var c = b.Cells[0];
            Assert.AreEqual("A1", c.Name);
            var cells = b.GetAllCellsInFile(c).ToList();
            Assert.AreEqual(8, cells.Count());
            Assert.AreEqual(1, cells.Count(x => x.Rank == "1"));
            Assert.AreEqual(8, cells.Count(x => x.File == "A"));
            Assert.AreEqual(8, cells.Select(x => x.Rank).Distinct().Count());
        }

        [TestMethod]
        public void PositiveDiagonalBottomLeft()
        {
            var b = new Board();
            var c = b.Cells[0];
            Assert.AreEqual("A1", c.Name);
            var cells = b.GetAllCellsOnPositiveDiagonal(c).ToList();
            Assert.AreEqual(8, cells.Select(x => x.Rank).Distinct().Count());
            Assert.AreEqual(8, cells.Select(x => x.File).Distinct().Count());
        }

        [TestMethod]
        public void NegativeDiagonalBottomLeft()
        {
            var b = new Board();
            var c = b.Cells[0];
            Assert.AreEqual("A1", c.Name);
            var cells = b.GetAllCellsOnNegativeDiagonal(c).ToList();
            Assert.AreEqual(1, cells.Count);
            Assert.AreSame(c, cells[0]);
        }

        [TestMethod]
        public void PositiveDiagonalTopRightCorner()
        {
            var b = new Board();
            var c = b.Cells.First(x => x.Name == "H8");
            var cells = b.GetAllCellsOnPositiveDiagonal(c).ToList();
            //Console.WriteLine(cells.PrintCells());
            Assert.AreEqual(8, cells.Select(x => x.Rank).Distinct().Count());
            Assert.AreEqual(8, cells.Select(x => x.File).Distinct().Count());
        }

        [TestMethod]
        public void PositiveDiagonalTopLeftCorner()
        {
            var b = new Board();
            var c = b.Cells.First(x => x.Name == "A8");
            var cells = b.GetAllCellsOnPositiveDiagonal(c).ToList();
            //Console.WriteLine(cells.PrintCells());
            Assert.AreEqual(1, cells.Count);
            Assert.AreSame(c, cells[0]);
        }
    }
}