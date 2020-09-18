using System;
using System.Collections.Generic;
using System.Text;
using ModuleHelper.Utility;

namespace ModuleHelper.Tests
{
    public class UnitTestDialogService : IDialogService
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}