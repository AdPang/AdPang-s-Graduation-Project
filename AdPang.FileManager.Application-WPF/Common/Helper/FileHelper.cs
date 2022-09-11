using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AdPang.FileManager.Application_WPF.Common.Models;

namespace AdPang.FileManager.Application_WPF.Common.Helper
{
    public static class FileHelper
    {
        public static ObservableCollection<LocalFilesInfo> FindRepateFileName(ObservableCollection<LocalFilesInfo> fileInfos)
        {
            var duplicateValues = fileInfos.GroupBy(x => x.File.Name).Where(x => x.Count() > 1);
            ObservableCollection<LocalFilesInfo> files = new();

            foreach (var duplicateValue in duplicateValues)
            {
                foreach (var file in duplicateValue)
                {
                    files.Add(file);
                }
            }
            if (files.Count == 0)
                return fileInfos;
            return files;
        }
        public static void FileMove(FileInfo file, string destPath)
        {
            file.MoveTo(destPath + "\\" + file.Name);
        }
        public static void FileRename(string mode, ICollection<LocalFilesInfo> fileInfos)
        {
            foreach (var item in fileInfos)
            {
                string name = "";
                if (mode.Equals("Upper"))
                {
                    name = item.File.Name.ToUpperInvariant();
                    if (item.File.Name.Contains('.'))
                    {
                        name = "";
                        var fileNameArray = item.File.Name.Split('.');
                        for (int i = 0; i < fileNameArray.Length; i++)
                        {
                            if (i == fileNameArray.Length - 1)
                                name += "." + fileNameArray[i].ToLowerInvariant();
                            else if (i == 0)
                                name += fileNameArray[i].ToUpperInvariant();
                            else
                                name += "." + fileNameArray[i].ToUpperInvariant();
                        }
                    }

                }
                else if (mode.Equals("Lower"))
                    name = item.File.Name.ToLowerInvariant();
                item.File.MoveTo(item.File.DirectoryName + "\\" + name);
            }
        }
        public static void FileRename(string rootPath, LocalFilesInfo selectedFileInfo)
        {
            var destPath = selectedFileInfo.File.Directory + "\\" + selectedFileInfo.FileName;
            selectedFileInfo.File.MoveTo(destPath);
        }

        public static void GetAllFileFromDir(DirectoryInfo dir, List<FileInfo> fileInfos)
        {
            var dirFiles = dir.GetFiles();
            if (dirFiles.Length > 0)
            {
                fileInfos.AddRange(dirFiles);
            }
            var dirChildrenDir = dir.GetDirectories();
            foreach (var dirChild in dirChildrenDir)
            {
                GetAllFileFromDir(dirChild, fileInfos);
            }
        }
        public static void OpenFile(string fullName)
        {
            Process process = new();
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fullName);
            process.StartInfo = processStartInfo;
            process.StartInfo.UseShellExecute = true;
            StartProcess(process);
        }

        internal static void OpenFileDir(string filePath)
        {
            Process process = new Process();
            ProcessStartInfo psi = new("Explorer.exe");
            process.StartInfo = psi;
            psi.Arguments = "/e,/select," + filePath;
            StartProcess(process);
        }

        private static void StartProcess(Process process)
        {
            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
