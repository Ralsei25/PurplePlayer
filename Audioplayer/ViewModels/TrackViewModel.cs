using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Audioplayer.Infrastructure;
using Audioplayer.Models;

namespace Audioplayer.ViewModels
{
    public class TrackViewModel : NotifyPropertyChanged
    {
        private MusicTrack _track;
        private Visibility _visibility;
        private bool _isFavorite;

        public TrackViewModel(MusicTrack track)
        {
            _track = track;
            Visibility = Visibility.Visible;
        }

        public MusicTrack MusicTrack => _track;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                _isFavorite = value;
                RaisePropertyChange("IsFavorite");
            }
        }
        public Visibility Visibility 
        {
            get { return _visibility; }
            set 
            {
                _visibility = value;
                var b = _track.MetaData.Year > 0 ? _track.MetaData.Year : 0;
                RaisePropertyChange("Visibility");
            }
        }
        public string Name => _track.Name;
    }
}
