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
            g.Move("B7", "B5");
            g.Move("A4", "B5");
            Console.WriteLine(g.Board.PrintBoard());
        }
    }
}
