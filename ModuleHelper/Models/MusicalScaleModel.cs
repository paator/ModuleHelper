using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ModuleHelper.ViewModels;

namespace ModuleHelper.Models
{
    public class MusicalScaleModel
    {
        public string Name { get; set; }
        public Note MainKey { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
    }
}