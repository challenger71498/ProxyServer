using System;
using System.IO;

namespace Min.MySqlProxyServer
{
    public class LoggerService
    {
        private const string LogPath = "./logs";

        private string GetFileName
        {
            get
            {
                var now = DateTime.Now;
                return now.ToString("y-MM-dd") + ".txt";
            }
        }

        public async void Log(string text)
        {
            var baseDir = AppContext.BaseDirectory;
            var fileName = this.GetFileName;

            var directoryPath = Path.GetFullPath(Path.Combine(baseDir, LogPath));
            var filePath = Path.GetFullPath(Path.Combine(directoryPath, fileName));

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            await File.AppendAllTextAsync(filePath, text + "\n", System.Text.Encoding.UTF8);
        }
    }
}