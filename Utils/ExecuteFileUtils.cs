using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class ExecuteFileUtils
    {
        public static void Execute(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new IOException("Execute file not found!");
                }
                string? folder = Path.GetDirectoryName(filePath);
                if (folder != null)
                {
                    string fileName = Path.GetFileName(filePath);
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.WorkingDirectory = folder;
                        process.StartInfo.RedirectStandardInput = true;
                        //process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                        process.StandardInput.WriteLine($"\"{fileName}\"");
                        process.StandardInput.WriteLine("exit");
                        //process.StandardOutput.ReadToEnd();
                        //process.WaitForExit();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}