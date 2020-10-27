using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ShellProgressBar;

namespace CliPlayer {
	public class Playlist {

		public List<FileInfo> MediaList { get; } = new List<FileInfo>();

		private Player Player { get; set; }

		public Playlist(IEnumerable<string> files) {
			MediaList.AddRange(files.Select(s => new FileInfo(s)));
			Player = new Player();
		}



		public async Task EventLoop() {
			int i = 1;
			var bar = new ProgressBar(MediaList.Count, "", new ProgressBarOptions { ProgressCharacter = '─' });
			foreach (var file in MediaList) {
				Player.PlayFile(file);
				bar.Tick($"File: {i}/{MediaList.Count}");
				Player.PlayPause();
				var fileBar = bar.Spawn((int)Player.TotalLength.TotalSeconds, file.Name, new ProgressBarOptions { CollapseWhenFinished = true });
				var next = await Player.TrackFinished(fileBar);
			}
		}
	}
}
