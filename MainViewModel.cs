using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TicTacToe.Commands;
using TicTacToe.Models;

namespace TicTacToe.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const int Size1 = 3;
        private const int Size2 = 3;
        private const string Player1Sign = "O";
        private const string Player2Sign = "X";
        private Bindable2DArray<Field> _board;
        private ObservableCollection<GamePlay> _gamePlays;
        private string _turnInfo;
        private string _gameInfo;
        private bool _isPlayer1Turn;
        private readonly SolveGame _solveGame = new SolveGame();
        private int _gameNumber = 1;
        public MainViewModel()
        {
            BoardClickCommand = new RelayCommand(BoardClick);
            ResetCommand = new RelayCommand(NewGame);
            ResetCommand = new RelayCommand(Reset);
        }

        private void Reset(object obj)
        {
            throw new NotImplementedException();
        }

        private void NewGame(object obj)
        {
            InitBoard();
            ChangePlayer();
        }

        private void ChangePlayer()
        {
            _isPlayer1Turn = !_isPlayer1Turn;
            var playerTurn = _isPlayer1Turn ? "O" : "X";
            TurnInfo = $"Oczekiwanie na: {playerTurn}.";
        }

        private void InitBoard()
        {
            Board = new Bindable2DArray<Field>(Size1, Size2);

            for (int i = 0; i < Size1; i++)
            {
                for (int j = 0; j < Size2; j++)
                    Board[i, j] = new Field
                    {
                        Content = "",
                        Background = Brushes.Transparent,
                        IsEnabled = true
                    };
            }
        }

        private void BoardClick(object obj)
        {
            var index = obj as string;
            var clickedField = GetFieldByIndex(index);

            var sign = _isPlayer1Turn ? Player1Sign : Player2Sign;

            clickedField.Content = sign;
            clickedField.IsEnabled = false;

            var gameResult = _solveGame.GetResult(
                new string[Size1, Size2]
                {
                    { Board[0,0].Content, Board[0,1].Content, Board[0,2].Content},
                    { Board[1,0].Content, Board[1,1].Content, Board[1,2].Content},
                    { Board[2,0].Content, Board[2,1].Content, Board[2,2].Content}
                });

            if (gameResult.Result == Result.GameInProgress)
            {
                ChangePlayer();
                return;
            }
            GameOver(gameResult, index);
        }

        private void GameOver(GameResult gameResult, string index)
        {
            if (gameResult.Result == Result.Draw)
            {
                GameInfo = "Remis.";
                GamePlays.Add(new GamePlay { Number = _gameNumber++, SignXInfo = "Remis",
                    SignOInfo = "Remis" });
            }
            else if (gameResult.Result == Result.WonX)
            {
                GameInfo = $"Wygrał: {Player2Sign}. Gratulacje.";
                GamePlays.Add(new GamePlay
                {
                    Number = _gameNumber++,
                    SignXInfo = "Wygrana",
                    SignOInfo = "Porażka"
                });
                DrawWinner(gameResult.WinnerType, index);
            }
            else if (gameResult.Result == Result.WonO)
            {
                GameInfo = $"Wygrał: {Player1Sign}. Gratulacje.";
                GamePlays.Add(new GamePlay
                {
                    Number = _gameNumber++,
                    SignXInfo = "Porażka",
                    SignOInfo = "Wygrana"
                });
                DrawWinner(gameResult.WinnerType, index);
            }
            TurnInfo = "Koniec gry.";
            DisableField();
        }

        private void DisableField()
        {
            for (int i = 0; i < Size1; i++)
                for (int j = 0; j < Size2; j++)
                    Board[i, j].IsEnabled = false;

        }
    


        private void DrawWinner(WinnerType winnerType, string index)
        {
            var rowNumber = index[0].ToString();
            var colNumber = index[2].ToString();

            switch (winnerType)
            {
                case WinnerType.None:
                    break;
                case WinnerType.Row:
                    Board[int.Parse(rowNumber), 0].Background = Brushes.Green;
                    Board[int.Parse(rowNumber), 0].Background = Brushes.Green;
                    Board[int.Parse(rowNumber), 0].Background = Brushes.Green;
                    break;
                case WinnerType.Column:
                    Board[0, int.Parse(columnNumber)].Background = Brushes.Green;
                    Board[1, int.Parse(columnNumber)].Background = Brushes.Green;
                    Board[2, int.Parse(columnNumber)].Background = Brushes.Green;
                    break;
                case WinnerType.Diagonal:
                    break;
                default:
                    break;
            }
        }

        private Field GetFieldByIndex(string index)
        {
            var parts = index.Split('-'); // wiemy ktore pole zostalo klikniete
            return Board[int.Parse(parts[0]), int.Parse(parts[1])];
        }

        public Bindable2DArray<Field> Board
        { 
            get
            {
                return _board;
            }
            set
            {
                _board = value;
                OnPropertyChanged();
            }
        }

        public string TurnInfo
        {
            get
            {
                return _turnInfo;
            }
            set
            {
                _turnInfo = value;
                OnPropertyChanged();
            }
        }

        public string GameInfo
        {
            get
            {
                return _gameInfo;
            }
            set
            {
                _gameInfo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GamePlay> GamePlays
        {
            get
            {
                return _gamePlays;
            }
            set
            {
                _gamePlays = value;
                OnPropertyChanged();
            }
        }

        public ICommand BoardClickCommand { get; set; }
        public ICommand NewGameCommand { get; set; }
        public ICommand ResetCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertychanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
