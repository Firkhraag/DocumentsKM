using System.Diagnostics;

namespace MyProcessSample
{
    class MyProcess
    {
        void OpenWithArguments()
        {
            // Can call a LaTeX interpreter with Process.Start to produce pdf

            Process latexProcess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo(@"\path\to\latex\latex.exe");
            startInfo.WorkingDirectory = @"\path\to\latex";
            startInfo.FileName = "latex";
            startInfo.Arguments = "file.tex";
            latexProcess.StartInfo = startInfo;
            latexProcess.Start();

            // ProcessStartInfo startInfo = new ProcessStartInfo(@"\path\to\latex\latex.exe");
            // startInfo.WorkingDirectory = @"\path\to\latex";

            // Process.Start("IExplore.exe", "www.northwindtraders.com");
        }
    }
}