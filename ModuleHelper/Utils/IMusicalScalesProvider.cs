using ModuleHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ModuleHelper.Utils
{
    public interface IMusicalScalesProvider
    {
        IEnumerable<MusicalScaleModel> LoadScales(string source);
    }
}