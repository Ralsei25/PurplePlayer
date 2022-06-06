using Audioplayer.Infrastructure;
using Audioplayer.Models;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Audioplayer.ViewModels
{
    public class TrackListViewModel : NotifyPropertyChanged
    {
        private readonly Player _player;
        private string _searchText = "";
        private TrackViewModel _selectedTrack = null;
        private ObservableCollection<TrackViewModel> _trackList = new ObservableCollection<TrackViewModel>();

        public TrackListViewModel(Player player)
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            OpenFolderCommand = new RelayCommand(OpenFolder);
            UpdateFavoritesCommand = new RelayCommand(UpdateFavorites);
            SelectTrackCommand = new RelayCommand(SelectTrack);
            DeleteTrackCommand = new RelayCommand(DeleteTrack, (b) => SelectedTrack != null);

            _player = player;
        }

        private void DeleteTrack(object obj)
        {
            TrackList.Remove(SelectedTrack);
            RaisePropertyChange("FavoriteTrackList");
        }

        private void SelectTrack(object param)
        {
            MusicTrack track = ((TrackViewModel)param).MusicTrack;
            _player.OpenTrack(track);
        }

        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand OpenFolderCommand { get; private set; }
        public RelayCommand UpdateFavoritesCommand { get; private set; }
        public RelayCommand SelectTrackCommand { get; private set; }
        public RelayCommand DeleteTrackCommand { get; private set; }
        public ObservableCollection<TrackViewModel> TrackList => _trackList;
        public ObservableCollection<TrackViewModel> FavoriteTrackList =>  new ObservableCollection<TrackViewModel>(TrackList.Where(x => x.IsFavorite));
        public TrackViewModel SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                _selectedTrack = value;
                RaisePropertyChange("SelectedTrack");
            }
        }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            if (SearchText.Length > 2)
            {
                foreach (var track in TrackList)
                {
                    if (track.Name.IndexOf(SearchText) > -1)
                    {
                        track.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        track.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                foreach (var track in TrackList)
                {
                    if (track.Visibility != Visibility.Visible)
                    {
                        track.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void UpdateFavorites(object param)
        {
            RaisePropertyChange("FavoriteTrackList");
        }
        private void OpenFile(object param)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Media files (*.mp3)|*.mp3"
            };
            if (ofd.ShowDialog() == true)
            {
                foreach (var track in ofd.FileNames)
                {
                    AddTrack(track);
                }
            }
        }
        private void OpenFolder(object param)
        {
            CommonOpenFileDialog ofd = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                AllowNonFileSystemItems = false,
                DefaultFileName = "Select Folder",
                Title = "Select Folder"
            };
            if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var dirPath = System.IO.Path.GetDirectoryName(ofd.FileName);
                var files = Directory.GetFiles(dirPath, "*.mp3");
                foreach (var track in files)
                {
                    AddTrack(track);
                }
            }
        }
        private void AddTrack(string filePath)
        {
            var file = new FileInfo(filePath);
            var tagFile = TagLib.File.Create(filePath);
            MetaData meta = new MetaData
            {
                Title = tagFile.Tag.Title,
                Album = tagFile.Tag.Album,
                Singer = String.Join(", ", tagFile.Tag.Performers),
                Year = tagFile.Tag.Year,
                Duration = tagFile.Properties.Duration
            };
            if (tagFile.Tag.Pictures.Length > 0)
            {
                MemoryStream ms = new MemoryStream(tagFile.Tag.Pictures.First().Data.Data);
                ms.Seek(0, SeekOrigin.Begin);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.EndInit();
                meta.Artwork = image;
            }

            var track = new MusicTrack() { FilePath = filePath, Name = file.Name, MetaData = meta };
            var trackVM = new TrackViewModel(track);
            TrackList.Add(trackVM);
        }
    }
}
