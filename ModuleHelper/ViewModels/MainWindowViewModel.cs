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

        public Array Notes { get => Enum.GetValues(typeof(Note)); }

        public MainWindowViewModel()
        {
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
                    Name = node.Attributes[0].Value,
                    Notes = new ObservableCollection<Note>()
                };

                //fill musical scale with notes from xml file
                var musicalNotes = node.SelectNodes("/Notes/Note");

                foreach (XmlNode note in musicalNotes)
                {
                    var musicalScaleNote = note.SelectSingleNode("Note").InnerText;

                    musicalScale.Notes.Add((Note) Enum.Parse(typeof(Note), musicalScaleNote));
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