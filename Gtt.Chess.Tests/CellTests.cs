using System;
using Gtt.Chess.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtt.Chess.Tests
{
    [TestClass]
    public class CellTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void XCannotBeLessThan1()
        {
            var cell = new Cell(0, 1);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void YCannotBeLessThan1()
        {
            var cell = new Cell(1, 0);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void YCannotBeMoreThan8()
        {
            var cell = new Cell(1, 9);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void XCannotBeMoreThan8()
        {
            var cell = new Cell(9, 1);
        }

        [TestMethod]
        public void FirstPositionIsA1()
        {
            var cell = new Cell(1, 1);
            Assert.AreEqual("A1", cell.Name);
        }

        [TestMethod]
        public void LastPositionIsH8()
        {
            var cell = new Cell(8, 8);
            Assert.AreEqual("H8", cell.Name);
        }
    }
}
