using Audioplayer.Infrastructure;
using Audioplayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audioplayer.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private Player _player;
        private TrackInfoViewModel _trackInfo;
        private ControlPanelViewModel _controlPanel;
        private TrackListViewModel _trackList;
        public MainWindowViewModel(Player player, ControlPanelViewModel controlPanel, TrackListViewModel trackList, TrackInfoViewModel trackInfo)
        {
            _player = player;
            _controlPanel = controlPanel;
            _trackList = trackList;
            _trackInfo = trackInfo;
        }

    }
}
