using System.Collections.Generic;
using System.ComponentModel;
using ModuleHelper.ViewModels;

namespace ModuleHelper.Models
{
    public class MusicalScaleModel
    {
        public string Name { get; set; }
        public Note MainKey { get; set; }
        public IList<Note> Notes { get; set; }
    }
}