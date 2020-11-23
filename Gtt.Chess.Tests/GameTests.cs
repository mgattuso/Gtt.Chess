using System;
using System.Collections.Generic;
using System.Text;
using Gtt.Chess.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtt.Chess.Tests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void DefaultGame()
        {
            var g = new Game(GameStyle.Traditional);
            g.Move("A2", "A4");
            Console.WriteLine(g.Board.PrintBoard());
        }

        [TestMethod]
        public void GameToCheck()
        {
            var g = new Game(GameStyle.Traditional);
            g.Move("E2", "E3");
            g.Move("F7", "F6");
            var a = g.Move("D1", "H5"); //CHECK
            Assert.AreEqual(1, a.PiecesMoved.Length);
            var r = g.Move("F6", "F5"); // THIS MOVE SHOULD NOT DO ANYTHING AS IT DOES GET OUT OF CHECK
            Assert.AreEqual(0, r.PiecesMoved.Length);
        }
    }
}
