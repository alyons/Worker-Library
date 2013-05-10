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
        public bool PuzzleSoved
        {
            get { return SudokuSolved(solvedPuzzle); }
        }
        #endregion

        #region Constructors
        public SudokuWorker(string puzzleBase)
            : base()
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrWhiteSpace(puzzleBase), "puzzleBase cannot be null, empty, or white space.");
            Contract.Requires<ArgumentException>(IsDigitsOnly(puzzleBase), "puzzleBase must only contain number.");
            Contract.Requires<ArgumentException>(puzzleBase.Count() == 81, "puzzleBase must be 81 characters long.");

            basePuzzle = new int[9, 9];
            workingPuzzle = new int[9, 9];
            solvedPuzzle = new int[9, 9];

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    basePuzzle[r, c] = puzzleBase[r + c * 9];
                    workingPuzzle[r, c] = puzzleBase[r + c * 9];
                    solvedPuzzle[r, c] = puzzleBase[r + c * 9];
                }
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
        private static bool SudokuSolved(int[,] sudoku)
        {
            Contract.Requires<ArgumentNullException>(sudoku != null, "sudoku cannot be null.");

            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                    if (sudoku[r, c] == 0) return false;
            
            return true;
        }
        void Solve(int row, int col)
        {
            if (row > 8)
            {
                solvedPuzzle = workingPuzzle;
                return;
            }

            if (workingPuzzle[row, col] != 0)
            {
                next(row, col);
            }
            else
            {
                for (int num = 1; num < 10; num++)
                {
                    if (CheckRow(row, num) && CheckColumn(col, num) && CheckSquare(row, col, num))
                    {
                        workingPuzzle[row, col] = num;
                        next(row, col);
                    }
                }
            }
        }
        void next(int row, int col)
        {
        }
        bool CheckRow(int row, int num)
        {
            for (int c = 0; c < 9; c++)
                if (workingPuzzle[row, c] == num)
                    return false;

            return true;
        }
        bool CheckColumn(int col, int num)
        {
            for (int r = 0; r < 9; r++)
                if (workingPuzzle[r, col] == num)
                    return false;

            return true;
        }
        bool CheckSquare(int row, int col, int num)
        {
            row = (row / 3) * 3;
            col = (col / 3) * 3;

            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (workingPuzzle[row + r, col + c] == num)
                        return false;

            return true;
        }
        void WriteToConsole()
        {
            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                    Console.Write(workingPuzzle[r, c] + " ");
        }
        #endregion
    }
}
