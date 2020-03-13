using System.Collections.Generic;
using System.ComponentModel;
using ModuleHelper.ViewModels;

namespace ModuleHelper.Models
{
    public class MusicalScale : INotifyPropertyChanged
    {
        private string _name;

        private IList<Note> _notes;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public IList<Note> Notes
        {
            get => _notes;
            set
            {
                if (!Equals(_notes, value))
                {
                    _notes = value;
                    RaisePropertyChanged("Notes");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}