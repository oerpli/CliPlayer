using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Utils;
using NAudio.Wave;
using ShellProgressBar;

namespace CliPlayer {

	public class Player {
		private string FileName { get; set; }
		AudioFileReader AudioFile { get; set; }
		WaveOutEvent WavePlayer { get; set; }

		public Player() {
			WavePlayer = new WaveOutEvent();
		}

		WaveOutEvent WaveDevice { get; set; }

		public void PlayFile(FileInfo file) {
			FileName = file.Name;
			WavePlayer.Stop();
			AudioFile?.Dispose();
			AudioFile = new AudioFileReader(file.FullName);
			WavePlayer.Init(AudioFile);
			TotalLength = AudioFile.TotalTime;
		}


		public void Pause() => WaveDevice.Pause();
		public void Stop() => WavePlayer.Stop();

		public void PlayPause() {
			Action fn = WavePlayer.PlaybackState switch
			{
				PlaybackState.Stopped => WavePlayer.Play,
				PlaybackState.Playing => WavePlayer.Pause,
				PlaybackState.Paused => WavePlayer.Play,
				_ => throw new ArgumentOutOfRangeException()
			};
			var t = Task.Run(() => {
				fn();
				while (WavePlayer.PlaybackState == PlaybackState.Playing) {
					Thread.Sleep(1000);
				}
			});
		}


		public void Back() { }

		public void FastForward() { }

		public Task<bool> TrackFinished(ChildProgressBar progressBar)
			=> Task.Run(() => {
				while (AudioFile.Position < AudioFile.Length) {
					progressBar.Tick();
					Thread.Sleep(1000);
				}
				return true;
			});

		public TimeSpan TotalLength { get; private set; }
		public TimeSpan Position => AudioFile.CurrentTime;
	}
}
