using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ModuleHelper.Models;

namespace ModuleHelper.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MusicalScaleModel> _musicalScales;
        private MusicalScaleModel _currentMusicalScale;
        private Note _currentNote;

        public Array Notes { get => Enum.GetValues(typeof(Note)); }

        public MainWindowViewModel()
        {
            _musicalScales = new ObservableCollection<MusicalScaleModel>();

            _currentMusicalScale = new MusicalScaleModel
            {
                Name = "Please select a Musical Scale",
                Notes = new ObservableCollection<Note>()
            };

            LoadScalesFromXml("musicalscales.xml");
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

                    //I update current scale's name and notes as well
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
                    _currentNote = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}