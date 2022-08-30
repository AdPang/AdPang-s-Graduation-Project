using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Application_WPF.Common.Helper
{
    public static class FileHelper
    {
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
