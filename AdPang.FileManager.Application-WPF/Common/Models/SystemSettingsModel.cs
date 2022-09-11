using System.Configuration;
using System.IO;

namespace AdPang.FileManager.Application_WPF.Common.Models
{
    public static class SystemSettingsModel
    {
        private static string downloadPath = string.Empty;

        static SystemSettingsModel()
        {
            downloadPath = System.Windows.Forms.Application.StartupPath + @"FileManagerDownloaded\";
            string? configPath = ConfigurationManager.AppSettings["DownloadPath"];
            if (string.IsNullOrEmpty(configPath))
            {
                return;
            }
            else
            {
                var dir = new DirectoryInfo(configPath);
                if (dir.Exists)
                {
                    downloadPath = configPath;
                }
            }
        }

        public static string GetDownloadPath()
        {
            return downloadPath;
        }
        public static string SetDownloadPath(string path)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.Exists) { return string.Empty; }
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["DownloadPath"].Value = dir.FullName;
            cfa.Save(ConfigurationSaveMode.Modified);
            downloadPath = dir.FullName;
            return downloadPath;
        }
    }
}
