using ModuleHelper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ModuleHelper.Services
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
