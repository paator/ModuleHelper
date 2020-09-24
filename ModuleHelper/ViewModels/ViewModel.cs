using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ModuleHelper.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
