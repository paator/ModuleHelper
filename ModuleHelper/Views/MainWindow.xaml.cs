using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ModuleHelper.Services;
using ModuleHelper.Utils;
using ModuleHelper.ViewModels;

namespace ModuleHelper.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            IMusicalScalesProvider scalesProvider = new MusicalScalesProvider();
            IDialogService dialogService = new DialogService();
            ISoundEngine soundEngine = new SoundEngine();

            DataContext = new MainWindowViewModel("musicalscales.xml", dialogService, scalesProvider, soundEngine);
            InitializeComponent();
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(var panel in Piano.Children)
            {
                if(panel is StackPanel stackPanel)
                {
                    foreach(var key in stackPanel.Children)
                    {
                        if (key is ToggleButton toggleButton)
                        {
                            toggleButton.IsChecked = false;
                        }
                    }
                }
            }
        }
    }
}