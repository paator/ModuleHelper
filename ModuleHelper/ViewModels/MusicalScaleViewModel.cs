using System.Collections.Generic;
using System.ComponentModel;
using ModuleHelper.Models;

namespace ModuleHelper.ViewModels
{
    public class MusicalScaleViewModel : INotifyPropertyChanged
    {
        private MusicalScaleModel _musicalscale;

        public MusicalScaleViewModel()
        {
            _musicalscale = new MusicalScaleModel();
        }

        public string Name
        {
            get => _musicalscale.Name;
            set
            {
                if (_musicalscale.Name != value)
                {
                    _musicalscale.Name = value;
                    OnPropertyChange("Name");
                }
            }
        }

        public Note MainKey
        {
            get => _musicalscale.MainKey;
            set
            {
                if (_musicalscale.MainKey != value)
                {
                    _musicalscale.MainKey = value;
                    OnPropertyChange("MainKey");
                }
            }
        }

        public IList<Note> Notes
        {
            get => _musicalscale.Notes;
            set
            {
                if (!Equals(_musicalscale.Notes, value))
                {
                    _musicalscale.Notes = value;
                    OnPropertyChange("Notes");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}