using ModuleHelper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ModuleHelper.Utility
{
    public class MessageBoxDialogService : IDialogService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
