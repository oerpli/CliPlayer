using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CliPlayer;
using ShellProgressBar;


public class Program {
	public static async Task Main(string[] args) {
		var path = @"Y:\GitRepos\CliPlayer\testalbum";
		var files = Directory.EnumerateFiles(path, "*.mp3", SearchOption.AllDirectories).ToList();
		var pl = new Playlist(files);
		await pl.EventLoop();
	}
}
