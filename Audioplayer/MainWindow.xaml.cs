using Audioplayer.Models;
using Audioplayer.ViewModels;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Audioplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var player = new Player();

            var controlPanelVM = new ControlPanelViewModel(player);
            var trackListVM = new TrackListViewModel(player);
            var trackInfoVM = new TrackInfoViewModel(player);

            ControlPanel.DataContext = controlPanelVM;
            TrackList.DataContext = trackListVM;
            TrackInfo.DataContext = trackInfoVM;

            var mainVM = new MainWindowViewModel(player, controlPanelVM, trackListVM, trackInfoVM);
            DataContext = mainVM;
        }
    }
}
