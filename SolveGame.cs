using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    class SolveGame
    {
        private string _winner;
        private WinnerType _type;
        public GameResult GetResult(string[,] board)
        {
            if (RowIsWon(board) || ColumnIsWon(board) || DiagonalIsWon(board))
                return new GameResult
                {
                    Result = _winner == "X" ? Result.WonX : Result.WonO,
                    WinnerType = _type
                };
                return GameIsInProgress(board);
        }

        private GameResult GameIsInProgress(string[,] board)
        {
            var result = board.Cast<string>().Contains(string.Empty);

            return result ?
                new GameResult { Result = Result.GameInProgress} :
                new GameResult { Result = Result.Draw };
        }

        private bool DiagonalIsWon(string[,] board)
        {
            var diagonal = GetDiagonal(board);

            for (int i = 0; i < 2; i++)
            {
                if (CheckIfGameIsWon(diagonal[i]))
                {
                    _winner = diagonal[i][0];
                    _type = WinnerType.Diagonal;
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfGameIsWon(List<string> list)
        {
            return list.All(x => x != string.Empty && x == list.First());
        }

        private List<List<string>> GetDiagonal(string[,] board)
        {
            var diagonal = new List<List<string>> { new List<string>(),
                new List<string>() };

            // x,o,x
            // x,x,x
            // o,o,o

            for (int i = 0; i < board.GetLength(0); i++)
            {
                diagonal[0].Add(board[i, i]);
            }

            for (int i = board.GetLength(0) - 1, j = 0; i >= 0; i--, j++)
            {
                diagonal[0].Add(board[i, j]);
            }

            return diagonal;
        }

        private bool ColumnIsWon(string[,] board)
        {
            for (int i = 0; i < board.GetLength(1); i++)
            {
                var column = GetColumn(board, i);

                if (CheckIfGameIsWon(column))
                {
                    _winner = column[0];
                    _type = WinnerType.Column;
                    return true;

                }
            }
            return false;
        }

        private List<string> GetColumn(string[,] board, int j)
        {
            var col = new List<string>();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                col.Add(board[i, j]);
            }

            return col;
        }

        private bool RowIsWon(string[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                var row = GetRow(board, i);

                if (CheckIfGameIsWon(row))
                {
                    _winner = row[0];
                    _type = WinnerType.Row;
                    return true;
                }
            }
            return false;
        }

        private List<string> GetRow(string[,] board, int i)
        {
            var row = new List<string>();

            for (int j = 0; j < board.GetLength(1); j++)
            {
                row.Add(board[i, j]);
            }
            return row;
        }
    }
}
