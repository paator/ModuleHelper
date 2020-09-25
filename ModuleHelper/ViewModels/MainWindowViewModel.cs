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
using ModuleHelper.Utility;
using System.IO;

namespace ModuleHelper.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region fields  
        private IDialogService _dialogService;
        private IMusicalScalesProvider _scalesProvider;
        //private IArpeggioPlayer _arpeggioPlayer;

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
        public Array Notes { get => Enum.GetValues(typeof(Note)); }

        public bool IsUsingHexNotation
        {
            get => _isUsingHexNotation;

            set
            {
                if (_isUsingHexNotation != value)
                {
                    
                    _isUsingHexNotation = value;
                    OnPropertyChange("IsUsingHexNotation");
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
                    OnPropertyChange("IsUsingScales");
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
                    OnPropertyChange("ArpDelayTime");
                }
            }
        }

        public ICommand PianoCommand
        {
            get
            {
                if (_pianoCommand == null)
                {
                    _pianoCommand = new RelayCommand(param => CalculateDistanceBetweenKeys(param), param => CheckIfKeyIsInScale(param));
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
                    _playCommand = new RelayCommand(param => PlayArpeggio(_pressedKeysNumbers, 3));
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
                    OnPropertyChange("MusicalScales");
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
                    OnPropertyChange("CurrentKeyDifferences");
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

                    //fixing position of keys relative to main key when selecting new scale
                    ChangeNotesRelativeToKey(CurrentMusicalScaleNotes[0]); 

                    //updating current scale's name and notes as well
                    OnPropertyChange("CurrentMusicalScale");
                    OnPropertyChange("CurrentMusicalScaleName");
                    OnPropertyChange("CurrentMusicalScaleNotes");
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

        public ObservableCollection<Note> CurrentMusicalScaleNotes
        {
            get => _currentMusicalScale.Notes;
            set
            {
                if(_currentMusicalScale.Notes != value)
                {
                    _currentMusicalScale.Notes = value;
                    OnPropertyChange("CurrentMusicalScaleNotes"); 
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
                    OnPropertyChange("CurrentNote");
                }
            }
        }
        #endregion properties

        #region constructor
        public MainWindowViewModel(string musicalScalesFile, IDialogService dialogService, IMusicalScalesProvider scalesProvider)
        {
            _dialogService = dialogService;
            _scalesProvider = scalesProvider;

            IEnumerable<MusicalScaleModel> loadedMusicalScales = LoadMusicalScales("musicalscales.xml");

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
        #endregion constructor

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
            catch(FileLoadException e)
            {
                _dialogService.ShowMessage(e.Message);
                Environment.Exit(-2);
            }

            return scales;
        }

        #region methods
        public void CalculateDistanceBetweenKeys(object param)
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
            }

            _pressedKeysNumbers.Sort();

            for (int i = 0; i < _pressedKeysNumbers.Count(); i++)
            {
                int difference = _pressedKeysNumbers[i] - _pressedKeysNumbers[0];

                if(_isUsingHexNotation)
                {
                    CurrentKeyDifferences.Add(difference.ToString("X"));
                }
                else
                {
                    CurrentKeyDifferences.Add(difference.ToString());
                }
            }

            var squareWave = new SignalGenerator()
            {
                Gain = 0.12,
                Frequency = MathUtils.CalculateFrequency(int.Parse((string)param)),
                Type = SignalGeneratorType.Square
            };

            var trimmed = new OffsetSampleProvider(squareWave);
            var trimmedWithTimeSpan = trimmed.Take(TimeSpan.FromSeconds(0.4));

            WaveformPlayer.Instance.PlayWaveform(trimmedWithTimeSpan);
        }

        public void PlayArpeggio(IEnumerable<int> chordKeyNumbers, int length)
        {
            if (!chordKeyNumbers.Any()) return;

            List<ISampleProvider> arpeggioInputs = new List<ISampleProvider>();

            for (int i = 0; i <= length; i++)
            {
                foreach (var num in chordKeyNumbers)
                {
                    var squareWave = new SignalGenerator()
                    {
                        Gain = 0.12,
                        Frequency = MathUtils.CalculateFrequency(num),
                        Type = SignalGeneratorType.Square
                    };

                    var trimmed = new OffsetSampleProvider(squareWave);
                    var trimmedWithTimeSpan = trimmed.Take(TimeSpan.FromSeconds(_maximumArpDelayTime - _arpDelayTime));
                    arpeggioInputs.Add(trimmedWithTimeSpan);
                }
            }

            var concatenatedWaveforms = arpeggioInputs[0];

            foreach (var input in arpeggioInputs.Skip(1)) 
            {
                concatenatedWaveforms = concatenatedWaveforms.FollowedBy(input);
            }

            WaveformPlayer.Instance.StopPlayback();
            WaveformPlayer.Instance.PlayWaveform(concatenatedWaveforms);
        }

        public bool CheckIfKeyIsInScale(object param)
        {
            if(!_isUsingScales)
            {
                return true;
            }
            else if (param is string s)
            {
                var number = int.Parse(s);
                return CurrentMusicalScaleNotes.Any(note => MathUtils.Modulo(number, 12) == (int)note);
            }
            else return false;
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
                newNoteIntValue = MathUtils.Modulo(newNoteIntValue, 12);

                Note newNote = (Note)newNoteIntValue;
                CurrentMusicalScaleNotes[i] = newNote;
            }
        }

        public void SwitchBetweenHexAndDec()
        {
            if (_isUsingHexNotation)
            {
                var enumeration = CurrentKeyDifferences.Select(x => int.Parse(x).ToString("X"));
                CurrentKeyDifferences = new ObservableCollection<string>(enumeration);
            }
            else
            {
                var enumeration = CurrentKeyDifferences.Select(x => Convert.ToInt64(x, 16).ToString());
                CurrentKeyDifferences = new ObservableCollection<string>(enumeration);
            }
        }
        #endregion methods
    }
}