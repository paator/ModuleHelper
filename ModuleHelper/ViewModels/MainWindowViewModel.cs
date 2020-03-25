using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ModuleHelper.Models;

namespace ModuleHelper.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MusicalScaleModel> _musicalScales;
        private MusicalScaleModel _currentMusicalScale;

        public MainWindowViewModel()
        {
            _musicalScales = new ObservableCollection<MusicalScaleModel>();

            var musicalScale1 = new MusicalScaleModel
            {
                Name = "Pentatonic",
                MainKey = Note.E
            };

            var musicalScale2 = new MusicalScaleModel
            {
                Name = "Minor Harmonic",
                MainKey = Note.Asharp
            };

            var musicalScale3 = new MusicalScaleModel
            {
                Name = "Major",
                MainKey = Note.C
            };

            _musicalScales.Add(musicalScale1);
            _musicalScales.Add(musicalScale2);
            _musicalScales.Add(musicalScale3);
        }


        public ObservableCollection<MusicalScaleModel> MusicalScales
        {
            get => _musicalScales;

            set
            {
                if(_musicalScales != value)
                {
                    _musicalScales = value;
                    OnPropertyChange("MusicalScales");
                }
            }
        }

        public MusicalScaleModel CurrentMusicalScale
        {
            get => _currentMusicalScale;

            set
            {
                if(_currentMusicalScale != value)
                {
                    _currentMusicalScale = value;
                    OnPropertyChange("CurrentMusicalScale");
                }
            }
        }

        public string CurrentMusicalScaleName
        {
            get => _currentMusicalScale.Name;
            set
            {
                if (_currentMusicalScale.Name != value)
                {
                    _currentMusicalScale.Name = value;
                    OnPropertyChange("CurrentMusicalScaleName");
                }
            }
        }

        public Note CurrentMusicalMainKey
        {
            get => _currentMusicalScale.MainKey;
            set
            {
                if (_currentMusicalScale.MainKey != value)
                {
                    _currentMusicalScale.MainKey = value;
                    OnPropertyChange("CurrentMusicalMainKey");
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