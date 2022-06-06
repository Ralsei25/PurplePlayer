using Audioplayer.Infrastructure;
using Audioplayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace Audioplayer.ViewModels
{
    public class TrackInfoViewModel : NotifyPropertyChanged
    {
        private readonly Player _player;
        private MusicTrack _track;
        public TrackInfoViewModel(Player player)
        {
            _player = player;
            _player.CurrentTrackChanged += (s, e) =>
            {
                Track = player.CurrentTrack;
            };
        }
        public MusicTrack Track
        {
            get => _track;
            set
            {
                _track = value;
                RaisePropertyChange("Track");
                RaisePropertyChange("Artwork");
                RaisePropertyChange("Name");
                RaisePropertyChange("FilePath");
                RaisePropertyChange("Album");
                RaisePropertyChange("Title");
                RaisePropertyChange("Singer");
                RaisePropertyChange("Year");
                RaisePropertyChange("Duration");
            }
        }
        public BitmapImage Artwork => Track?.MetaData.Artwork;
        public string Name => Track?.Name;
        public string FilePath => Track?.FilePath;
        public string Album => $"Альбом: {Track?.MetaData.Album}";
        public string Title => $"Название: {Track?.MetaData.Title}";
        public string Singer => $"Исполнитель: {Track?.MetaData.Singer}";
        public string Year
        {
            get
            {
                string year = Track?.MetaData.Year > 0 ? Track?.MetaData.Year.ToString() : "";
                return $"Год: {year}";
            }
        }
        public string Duration => $"Длительность: {Track?.MetaData.Duration.ToString(@"mm\:ss")}";
    }
}
