using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using ModuleHelper.Models;
using System.Windows.Input;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System.Windows;
using ModuleHelper.Utils;
using System.IO;
using ModuleHelper.Extensions;
using ModuleHelper.Services;
using ModuleHelper.Helpers;

namespace ModuleHelper.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region fields  
        private IDialogService _dialogService;
        private IMusicalScalesProvider _scalesProvider;
        private ISoundEngine _soundEngine;

        private ObservableCollection<MusicalScaleModel> _musicalScales;
        private ObservableCollection<string> _currentKeyDifferences;
        private List<int> _pressedKeysNumbers;
        private MusicalScaleModel _currentMusicalScale;
        private Note _currentNote;
        private ICommand _pianoCommand;
        private ICommand _clearCommand;
        private ICommand _playCommand;
        private ICommand _stopCommand;
        private bool _isUsingScales;
        private bool _isUsingHexNotation = true;
        private double _arpDelayTime;
        private const double _maximumArpDelayTime = 0.21;
        private const double _minimumArpDelayTime = 0.05;
        #endregion fields  

        #region properties
        public Array Notes => Enum.GetValues(typeof(Note));

        public bool IsUsingHexNotation
        {
            get => _isUsingHexNotation;

            set
            {
                if (_isUsingHexNotation != value)
                {
                    
                    _isUsingHexNotation = value;
                    OnPropertyChange(nameof(IsUsingHexNotation));
                    SwitchBetweenHexAndDec();
                }
            }
        }

        public bool IsUsingScales
        {
            get => _isUsingScales;

            set
            {
                if(_isUsingScales != value)
                {
                    _isUsingScales = value;
                    OnPropertyChange(nameof(IsUsingScales));
                }
            }
        }

        public double ArpDelayTime
        {
            get => _arpDelayTime;

            set
            {
                if (_arpDelayTime != value)
                {
                    _arpDelayTime = value;
                    OnPropertyChange(nameof(ArpDelayTime));
                }
            }
        }

        public ICommand PianoCommand
        {
            get
            {
                if (_pianoCommand == null)
                {
                    _pianoCommand = new RelayCommand(param => PianoCommandExecute(param), param => PianoCommandCanExecute(param));
                }

                return _pianoCommand;
            }

            set
            {
                _pianoCommand = value;
            }
        }

        public ICommand StopCommand
        {
            get
            {
                if(_stopCommand == null)
                {
                    _stopCommand = new RelayCommand(param => WaveformPlayer.Instance.StopPlayback());
                }

                return _stopCommand;
            }

            set
            {
                _stopCommand = value;
            }
        }
        
        public ICommand ClearCommand
        {
            get
            {
                if(_clearCommand == null)
                {
                    _clearCommand = new RelayCommand(param => ClearChord());
                }

                return _clearCommand;
            }

            set
            {
                _clearCommand = value;
            }
        }

        public ICommand PlayCommand
        {
            get
            {
                if (_playCommand == null)
                {
                    _playCommand = new RelayCommand(param => PlayExecute(_pressedKeysNumbers));
                }

                return _playCommand;
            }

            set
            {
                _playCommand = value;
            }
        }

        public ObservableCollection<MusicalScaleModel> MusicalScales
        {
            get => _musicalScales;

            set
            {
                if(_musicalScales != value)
                {
                    _musicalScales = value;
                    OnPropertyChange(nameof(MusicalScales));
                }
            }
        }

        public ObservableCollection<string> CurrentKeyDifferences
        {
            get
            {
                return _currentKeyDifferences;
            }
            set
            {
                if(_currentKeyDifferences != value)
                {
                    _currentKeyDifferences = value;
                    OnPropertyChange(nameof(CurrentKeyDifferences));
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

                    ChangeNotesRelativeToKey(CurrentMusicalScaleNotes[0]); 

                    OnPropertyChange(nameof(CurrentMusicalScale));
                    OnPropertyChange(nameof(CurrentMusicalScaleName));
                    OnPropertyChange(nameof(CurrentMusicalScaleNotes));
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
                    OnPropertyChange(nameof(CurrentMusicalScaleName));
                }
            }
        }

        public ObservableCollection<Note> CurrentMusicalScaleNotes
        {
            get => _currentMusicalScale.Notes;
            set
            {
                if(_currentMusicalScale.Notes != value)
                {
                    _currentMusicalScale.Notes = value;
                    OnPropertyChange(nameof(CurrentMusicalScaleNotes)); 
                }
            }
        }

        public Note CurrentNote
        {
            get => _currentNote;
            set
            {
                if(_currentNote != value)
                {
                    var previouslySelectedNote = _currentNote;
                    _currentNote = value;
                    ChangeNotesRelativeToKey(previouslySelectedNote);
                    OnPropertyChange(nameof(CurrentNote));
                }
            }
        }
        #endregion properties
        public MainWindowViewModel(string musicalScalesFile, IDialogService dialogService, IMusicalScalesProvider scalesProvider, ISoundEngine soundEngine)
        {
            _dialogService = dialogService;
            _scalesProvider = scalesProvider;
            _soundEngine = soundEngine;

            IEnumerable<MusicalScaleModel> loadedMusicalScales = LoadMusicalScales(musicalScalesFile);

            _musicalScales = new ObservableCollection<MusicalScaleModel>(loadedMusicalScales);
            _pressedKeysNumbers = new List<int>();
            _currentKeyDifferences = new ObservableCollection<string>();
            _arpDelayTime = _minimumArpDelayTime;
            _currentMusicalScale = new MusicalScaleModel
            {
                Name = default,
                Notes = new ObservableCollection<Note>()
            };
        }

        public IEnumerable<MusicalScaleModel> LoadMusicalScales(string source)
        {
            IEnumerable<MusicalScaleModel> scales = new ObservableCollection<MusicalScaleModel>();

            try
            {
                scales = _scalesProvider.LoadScales(source);
            }
            catch (FileNotFoundException e)
            {
                _dialogService.ShowMessage(e.Message);
                Environment.Exit(-1);
            }
            catch (FileLoadException e)
            {
                _dialogService.ShowMessage(e.Message);
                Environment.Exit(-2);
            }

            return scales;
        }

        #region methods
        public void PianoCommandExecute(object param)
        {
            CurrentKeyDifferences.Clear();

            if (param is string s)
            {
                var number = int.Parse(s);
                if (_pressedKeysNumbers.Contains(number))
                {
                    _pressedKeysNumbers.Remove(number);
                }
                else
                {
                    _pressedKeysNumbers.Add(number);
                }

                _soundEngine.PlayKey(number);
            }

            var differences = MusicalMathHelper.CalculateKeyDifferences(_pressedKeysNumbers);

            CurrentKeyDifferences = differences.ToStringHex(_isUsingHexNotation);
        }

        public bool PianoCommandCanExecute(object param)
        {
            if (param is string s)
            {
                var keyNumber = int.Parse(s);
                return MusicalMathHelper.CheckIfKeyIsInScale(_isUsingScales, keyNumber, CurrentMusicalScaleNotes);
            }

            else return false;
        }

        public void PlayExecute(IEnumerable<int> pressedKeysNumbers)
        {
            var speed = _maximumArpDelayTime - _arpDelayTime;
            _soundEngine.PlayArpeggio(pressedKeysNumbers, 3, speed);
        }

        public void ClearChord()
        {
            _pressedKeysNumbers.Clear();
            CurrentKeyDifferences.Clear();
        }

        public void ChangeNotesRelativeToKey(Note previouslySelectedNote)
        {
            var selectedNoteIndex = (int)CurrentNote;
            var previouslySelectedNoteIndex = (int)previouslySelectedNote;

            //we calculate offset to know how to move every note in our musical scale
            //based on currently selected note
            var offset = selectedNoteIndex - previouslySelectedNoteIndex;

            if (offset == 0) return;
            
            for (var i = 0; i < CurrentMusicalScaleNotes.Count; i++)
            {
                var currentMusicalScaleNoteValue = (int)CurrentMusicalScaleNotes[i];
                int newNoteIntValue;

                newNoteIntValue = currentMusicalScaleNoteValue + offset;

                //12 -> back to C note, just higher octave
                //0 -> back to B note, just lower octave
                newNoteIntValue = MusicalMathHelper.Modulo(newNoteIntValue, 12);

                Note newNote = (Note)newNoteIntValue;
                CurrentMusicalScaleNotes[i] = newNote;
            }
        }

        public void SwitchBetweenHexAndDec()
        {
            CurrentKeyDifferences.SwitchBetweenHexAndDec(_isUsingHexNotation);
        }
        #endregion methods
    }
}