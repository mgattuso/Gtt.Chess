using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtt.Chess.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gtt.Chess.Tests
{
    [TestClass]
    public class Game2Tests
    {
        [TestMethod]
        public void BoardReferenceDefault()
        {
            var g = new Game(GameStyle.Traditional);
            g.Move("A2", "A4");
            g.Move("B7", "B5");
            g.Move("A4", "B5");
            g.Move("A7", "A6");
            g.Move("A1", "A6");
            var r = g.Move("A8", "A6");
            Assert.IsTrue(r.ValidMove);
            var serial = g.Board.SerializeBoardPositions().GroupBy(c => c[1]);
            foreach (var sz in serial.OrderByDescending(x => x.Key))
            {
                foreach (var z in sz)
                {
                    Console.Write($"{z,6}");
                }
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void PossibleMoves()
        {
            var g = new Game(GameStyle.Traditional);
            var moves = g.GetPossibleMoves("A2");
            Console.WriteLine(string.Join(", ", moves));
        }

        [TestMethod]
        public void TestCheck()
        {
            var g = new Game(GameStyle.Traditional);
            g.Move("E2", "E3");
            g.Move("F7", "F6");
            var m = g.Move("D1", "H5");
            Assert.IsTrue(m.ValidMove);
        }

        [TestMethod]
        public void CanCloneGame()
        {
            var g = new Game(GameStyle.Traditional);
            var clone = g.Clone();
            var s = string.Join(", ", g.Board.SerializeBoardPositions());
            var c = string.Join(", ", clone.Board.SerializeBoardPositions());
            Console.WriteLine(s);
            Console.WriteLine(c);
            Assert.AreEqual(s, c);
        }
    }
}
