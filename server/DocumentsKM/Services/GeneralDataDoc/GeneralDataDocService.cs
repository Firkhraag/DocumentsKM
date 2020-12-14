using DocumentsKM.Models;
using DocumentsKM.Data;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Linq;

namespace DocumentsKM.Services
{
    public class GeneralDataDocService : IGeneralDataDocService
    {
        private IMarkGeneralDataPointRepo _markGeneralDataPointRepo;

        public GeneralDataDocService(IMarkGeneralDataPointRepo markGeneralDataPointRepo)
        {
            _markGeneralDataPointRepo = markGeneralDataPointRepo;
        }

        public async Task<MemoryStream> GetDocByMarkId(int markId)
        {
            var markGeneralDataPoints = _markGeneralDataPointRepo.GetAllByMarkId(markId);
            var memory = new MemoryStream();
            var docStr = TexTemplate.docTop;

            // var currentSectionId = -1;
            foreach (var point in markGeneralDataPoints)
            {
                // point.Text = point.Text.Replace("a", "b");
                // if (point.Section.Id != currentSectionId)
                // {
                //     currentSectionId = point.Section.Id;
                //     docStr += $"\\item {point.Text}\n";
                // }
                if (point.Text.Length > 1)
                {
                    if ((point.Text[0] == '#' || point.Text[0] == '-') && point.Text[1] == ' ')
                    {
                        point.Text = "\\item" + point.Text.Substring(1);
                    }
                }
                docStr += $"\\item {point.Text}\n";
                // if (section.Itemize.Count() != 0)
                // {
                //     docStr += "\\begin{itemize}";
                //     foreach (var item in section.Itemize)
                //         docStr += $"\\item {item}\n";
                //     docStr += "\\end{itemize}";
                // }
                // if (section.Enumerate.Count() != 0)
                // {
                //     docStr += "\\begin{enumerate}";
                //     foreach (var item in section.Enumerate)
                //         docStr += $"\\item {item}\n";
                //     docStr += "\\end{enumerate}";
                // }
            }

            docStr += TexTemplate.docBottom;

            Byte[] b = new UTF8Encoding(true).GetBytes(docStr);
            await memory.WriteAsync(b, 0, b.Length);
            memory.Position = 0;
            return memory;
        }
    }
}
