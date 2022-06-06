using Audioplayer.Infrastructure;
using Audioplayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace Audioplayer.ViewModels
{
    public class ControlPanelViewModel : NotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Player _player;
        public ControlPanelViewModel(Player player)
        {
            PlayPauseCommand = new RelayCommand(PlayPause, (b) => _player.IsOpened);
            StopCommand = new RelayCommand(Stop, (b) => _player.IsOpened);
            RewindCommand = new RelayCommand(Rewind, (b) => _player.IsOpened);
            RewindVolumeCommand = new RelayCommand(RewindVolume);
            MuteUnmuteCommand = new RelayCommand(MuteUnmute);

            _player = player;
            Volume = player.Volume;

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Start();

            _player.CurrentTrackChanged += (s, e) =>
            {
                Position = 0;
            };
            _player.OnPlay += (s, e) =>
            {
                RaisePropertyChange("IsPlaying");
                RaisePropertyChange("IsPause");
            };
            _player.OnPause += (s, e) =>
            {
                RaisePropertyChange("IsPlaying");
                RaisePropertyChange("IsPause");
            };
        }
        public bool IsPlaying => _player.IsPlaying;
        public bool IsPause => !IsPlaying;
        public bool IsMute => _player.IsMute;
        public bool IsUnmute => !IsMute;
        public string PositionDuration => $"{_player.Position.ToString(@"mm\:ss")}/{_player.Duration.ToString(@"mm\:ss")}";
        public double Position
        {
            get => _player.Position / _player.Duration;
            set
            {
                RewindByPosition(value);
                RaisePropertyChange("Position");
                RaisePropertyChange("PositionDuration");
            }
        }
        public double Volume
        {
            get => _player.Volume;
            set
            {
                SetVolume(value);
                RaisePropertyChange("Volume");
            }
        }

        private void SetVolume(double value)
        {
            _player.Volume = value;
        }

        public RelayCommand PlayPauseCommand { get; private set; }
        public RelayCommand StopCommand { get; private set; }
        public RelayCommand RewindCommand { get; private set; }
        public RelayCommand RewindVolumeCommand { get; private set; }
        public RelayCommand MuteUnmuteCommand { get; private set; }
        private void PlayPause(object param)
        {
            _player.PlayPause();
        }
        private void Stop(object param)
        {
            _player.Stop();
            Position = 0;
        }
        private void RewindByPosition(double value)
        {
            _player.SetPosition(value);
        }
        private void Rewind(object param)
        {
            int value = Int32.Parse((string)param);
            if (value > 0)
            {
                _player.Rewind(TimeSpan.FromSeconds(value));
            }
            else
            {
                _player.Rewind(-TimeSpan.FromSeconds(value), true);
            }
            Position = _player.Position / _player.Duration;
        }
        private void RewindVolume(object param)
        {
            double value = (double)param;
            _player.RewindVolume(value);
        }
        private void MuteUnmute(object param)
        {
            _player.MuteUnmute();
            RaisePropertyChange("IsMute");
            RaisePropertyChange("IsUnmute");
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (_player.IsOpened)
            {
                RaisePropertyChange("Position");
                RaisePropertyChange("PositionDuration");
            }
            RaisePropertyChange("IsPlaying");
        }
    }
}
