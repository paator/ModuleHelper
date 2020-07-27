using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using ModuleHelper.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace ModuleHelper.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MusicalScaleModel> _musicalScales;
        private MusicalScaleModel _currentMusicalScale;
        private Note _currentNote;
        private ICommand _pianoCommand;
        private List<int> _pressedKeysNumbers;
        private ObservableCollection<string> _currentKeyDifferencesInHex;

        public Array Notes { get => Enum.GetValues(typeof(Note)); }

        public MainWindowViewModel()
        {
            _musicalScales = new ObservableCollection<MusicalScaleModel>();
            _pressedKeysNumbers = new List<int>();
            _currentKeyDifferencesInHex = new ObservableCollection<string>();
            _currentMusicalScale = new MusicalScaleModel
            {
                Name = default,
                Notes = new ObservableCollection<Note>()
            };

            LoadScalesFromXml("musicalscales.xml");
        }

        public ICommand PianoCommand
        {
            get
            {
                if (_pianoCommand == null)
                {
                    _pianoCommand = new RelayCommand(param => CalculateDistanceBetweenKeys(param));
                }

                return _pianoCommand;
            }

            set
            {
                _pianoCommand = value;
            }
        }

        public void CalculateDistanceBetweenKeys(object param)
        {
            CurrentKeyDifferences.Clear();

            if(param is string s)
            {
                var number = Int16.Parse(s);
                _pressedKeysNumbers.Add(number);
            }

            for(int i = 0; i < _pressedKeysNumbers.Count(); i++)
            {
                int difference = _pressedKeysNumbers[i] - _pressedKeysNumbers[0];
                CurrentKeyDifferences.Add(difference.ToString("X"));
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
                return _currentKeyDifferencesInHex;
            }
            set
            {
                if(_currentKeyDifferencesInHex != value)
                {
                    _currentKeyDifferencesInHex = value;
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

        public void LoadScalesFromXml(string filePath)
        {
            XmlDocument document = new XmlDocument();

            document.Load(filePath);

            XmlNodeList nodes = document.DocumentElement.SelectNodes("/MusicalScales/MusicalScale");

            _musicalScales = new ObservableCollection<MusicalScaleModel>();

            foreach(XmlNode node in nodes)
            {
                //create new musicalscale of name set in xml node attribute
                var musicalScale = new MusicalScaleModel
                {
                    Name = node.Attributes["name"].Value,
                    Notes = new ObservableCollection<Note>()
                };

                //fill musical scale with notes from xml file
                var musicalNotesOfScale = node.ChildNodes;

                foreach (XmlNode note in musicalNotesOfScale)
                {
                    var musicalScaleNote = note.InnerText;
                    var parsedNoteValue = (Note) Enum.Parse(typeof(Note), musicalScaleNote);

                    musicalScale.Notes.Add(parsedNoteValue);
                }

                MusicalScales.Add(musicalScale);
            }
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
                newNoteIntValue = Modulo(newNoteIntValue, 12);

                Note newNote = (Note)newNoteIntValue;
                CurrentMusicalScaleNotes[i] = newNote;
            }
        }

        private int Modulo(int x, int m)
        {
            return (x % m + m) % m;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}