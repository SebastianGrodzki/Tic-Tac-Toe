using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TicTacToe.Models
{
    //nie się zbindować Content z labelki do tablicy 2 wymiarowej dlatego ta  klasa
    public class Bindable2DArray<T> : INotifyPropertyChanged // na róznych typach danych
                                        //Widok informowany o wszystkich zmianach w naszej tablicy
    {
        private readonly T[,] _data;
        public Bindable2DArray(int size1, int size2)
        {
            _data = new T[size1, size2];
        }

        public T this[int c1, int c2]
        {
            get
            {
                return _data[c1, c2]; //zrzucenie wartości z naszej tablicy
            }
            set
            {
                _data[c1, c2] = value;
                OnPropertyChanged(Binding.IndexerName);
            }
        }

        public T this[string stringIndex]
        {
            get
            {
                var index = GetIndexes(stringIndex); //zmapowanie tego indexu ktory jest stringiem na dwa indexy
                return _data[index.Item1, index.Item2]; //które są intami //zwracamy wartosc
            }
            set
            {
                var index = GetIndexes(stringIndex);
                _data[index.Item1, index.Item2] = value; //ustawiamy wartosc
                OnPropertyChanged(Binding.IndexerName);
            }
        }

        private (int, int) GetIndexes(string stringIndex)
        {
            var parts = stringIndex.Split('-');

            if (parts.Length != 2)
                throw new ArgumentException("The provided index is not valid.");

                return (int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
