using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TicTacToe.Models
{
    public class Field : INotifyPropertyChanged
    {
        private string _content;
        private bool _isEnabled;
        private Brush _background;
        public string Content
        {
            get 
            { 
                return _content; 
            }
            set 
            { 
                _content = value; 
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public Brush Background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs
                (propertyName));
        }
    }
}
