using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkerLibrary;
using System.Diagnostics.Contracts;

namespace WorkerLibraryTestApp
{
    public class SudokuWorker : PausableWorker
    {
        #region Variables
        int[,] basePuzzle;
        int[,] workingPuzzle;
        int[,] solvedPuzzle;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public SudokuWorker(string puzzleBase)
            : base()
        {
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(puzzleBase), "puzzleBase cannot be null, empty, or white space.");
            Contract.Requires<ArgumentException>(IsDigitsOnly(puzzleBase), "puzzleBase must only contain number.");
            Contract.Requires<ArgumentException>(puzzleBase.Count() == 81, "puzzleBase must be 81 characters long.");

            for (int r = 0; r < 9; r++)
            {
            }
        }
        #endregion

        #region Methods
        public override string WorkerType()
        {
            return String.Format("SudokuWorker[{0}]", ID);
        }

        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        #endregion
    }
}
