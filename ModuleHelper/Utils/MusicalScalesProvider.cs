using ModuleHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;

namespace ModuleHelper.Utils
{
    public class MusicalScalesProvider : IMusicalScalesProvider
    {
        public IEnumerable<MusicalScaleModel> LoadScales(string source)
        {
            XmlDocument document = new XmlDocument();

            document.Load(source);

            XmlNodeList nodes = document.DocumentElement.SelectNodes("/MusicalScales/MusicalScale");

            var scales = new ObservableCollection<MusicalScaleModel>();

            foreach (XmlNode node in nodes)
            {
                var musicalScale = new MusicalScaleModel
                {
                    Name = node.Attributes["name"].Value,
                    Notes = new ObservableCollection<Note>()
                };

                var musicalNotesOfScale = node.ChildNodes;

                foreach (XmlNode note in musicalNotesOfScale)
                {
                    var musicalScaleNote = note.InnerText;
                    var parsedNoteValue = (Note)Enum.Parse(typeof(Note), musicalScaleNote);

                    musicalScale.Notes.Add(parsedNoteValue);
                }

                scales.Add(musicalScale);
            }

            return scales;
        }
    }
}