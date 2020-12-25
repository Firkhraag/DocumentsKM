using System.Diagnostics;

namespace DocumentsKM
{
    static public class MyProcess
    {
        static public void OpenWithArguments()
        {
            // Can call a LaTeX interpreter with Process.Start to produce pdf

            // Process latexProcess = new Process();
            // ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\Program Files\texlive2019\2019\bin\win32\latex.exe");
            // startInfo.WorkingDirectory = @"D:\Downloads";
            // startInfo.FileName = "1";
            // startInfo.Arguments = "1.tex";
            // latexProcess.StartInfo = startInfo;
            // latexProcess.Start();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            // startInfo.UseShellExecute = false;
            startInfo.FileName = @"D:\Program Files\texlive2019\2019\bin\win32\pdflatex.exe";
            // startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = @"D:\Downloads\1.tex";

            Process latexProcess = new Process();
            latexProcess.StartInfo = startInfo;
            latexProcess.Start();

            // ProcessStartInfo startInfo = new ProcessStartInfo(@"\path\to\latex\latex.exe");
            // startInfo.WorkingDirectory = @"\path\to\latex";

            // Process.Start("IExplore.exe", "www.northwindtraders.com");
        }
    }
}