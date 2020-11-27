using System.Diagnostics;

namespace MyProcessSample
{
    class MyProcess
    {
        void OpenWithArguments()
        {
            // Can call a LaTeX interpreter with Process.Start to produce pdf
            Process.Start("IExplore.exe", "www.northwindtraders.com");
        }
    }
}