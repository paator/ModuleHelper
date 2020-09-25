using ModuleHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ModuleHelper.Utility
{
    public interface IMusicalScalesProvider
    {
        public IEnumerable<MusicalScaleModel> LoadScales(string source);
    }
}