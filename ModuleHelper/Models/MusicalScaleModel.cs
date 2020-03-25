using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using ModuleHelper.ViewModels;

namespace ModuleHelper.Models
{
    public class MusicalScaleModel
    {
        public string Name { get; set; }
        public ObservableCollection<Note> Notes { get; set; } //relative to C key
    }
}