using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace WorkerLibraryTestApp
{
    [TestFixture]
    public class SudokuWorkerTest
    {
        [Test]
        public void CreateSudoku()
        {
            SudokuWorker sudokuWorker = new SudokuWorker("200000060000075030048090100000300000300010009000008000001020570080730000090000004");

            Assert.AreEqual(false, sudokuWorker.PuzzleSoved);
        }
    }
}
