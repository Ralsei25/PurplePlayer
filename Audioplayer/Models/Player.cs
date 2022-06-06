using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Windows.Media;

namespace Audioplayer.Models
{
    public class Player
    {
        private MusicTrack _currentTrack;
        private readonly MediaPlayer _player;
        private bool _isOpened;
        private bool _isPlaying;
        public Player()
        {
            _player = new MediaPlayer();
            _player.MediaOpened += (s, e) =>
            {
                _isOpened = true;
                CurrentTrackChanged?.Invoke(this, EventArgs.Empty);
            };
            _player.MediaFailed += (s, e) =>
            {
                _isOpened = false;
                CurrentTrackChanged?.Invoke(this, EventArgs.Empty);
            };
        }
        public event EventHandler CurrentTrackChanged;
        public event EventHandler OnPlay;
        public event EventHandler OnPause;
        public MusicTrack CurrentTrack
        {
            get { return _currentTrack; }
            private set 
            {
                _currentTrack = value;
            }
        }
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    if (_isPlaying)
                    {
                        OnPlay?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        OnPause?.Invoke(this, EventArgs.Empty);
                    }
                }

            }
        }
        public bool IsMute => _player.IsMuted;
        public TimeSpan Position
        {
            get => _player.Position;
            set => _player.Position = value;
        }
        public double Volume
        {
            get => _player.Volume;
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 1)
                    value = 1;
                _player.Volume = value;
            }
        }
        public TimeSpan Duration
        {
            get
            {
                if (_player.NaturalDuration.HasTimeSpan)
                    return _player.NaturalDuration.TimeSpan;
                else
                    return new TimeSpan();
            }
        }

        public bool IsOpened => _isOpened;

        public void OpenTrack(MusicTrack track)
        {
            _player.Open(new Uri(track.FilePath));
            CurrentTrack = track;
        }

        public void PlayPause()
        {
            if (IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }
        public void Stop()
        {
            _player.Stop();
            IsPlaying = false;
        }

        public void Rewind(TimeSpan value, bool isNegative = false)
        {
            if (IsPlaying)
            {
                Pause();
                if (!isNegative)
                {
                    _player.Position += value;
                }
                else
                {
                    _player.Position -= value;
                }
                Play();
            }
            else
            {
                if (!isNegative)
                {
                    _player.Position += value;
                }
                else
                {
                    _player.Position -= value;
                }
            }
        }

        public void RewindVolume(double value)
        {
            _player.Volume += value;
        }

        public void MuteUnmute()
        {
            _player.IsMuted = !_player.IsMuted;
        }
        public void SetPosition(double value)
        {
            if (!_isOpened)
            {
                return;
            } 
            if (value < 0)
            {
                value = 0;       
            }
            if (value > 1)
            {
                value = 1;
            }
            if (IsPlaying)
            {
                Pause();
                _player.Position = _player.NaturalDuration.TimeSpan * value;
                Play();
            }
            else
            {
                _player.Position = _player.NaturalDuration.TimeSpan * value;
            }
        }
        private void Play()
        {
            _player.Play();
            IsPlaying = true;
        }
        private void Pause()
        {
            _player.Pause();
            IsPlaying = false;
        }
    }
}
