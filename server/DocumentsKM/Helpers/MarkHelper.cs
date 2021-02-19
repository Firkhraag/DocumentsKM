using System.Linq;
using System.Text;

namespace DocumentsKM.Helpers
{
    public static class MarkHelper
    {
        public static string MakeMarkName(
            string projectBaseSeries,
            string nodeCode,
            string subnodeCode,
            string markCode)
        {
            var markName = new StringBuilder(projectBaseSeries, 255);
            var overhaul = "";
            if (nodeCode != "-" && nodeCode != "")
            {
                var nodeCodeSplitted = nodeCode.Split('-');
                var nodeValue = nodeCodeSplitted[0];
                if (nodeCodeSplitted.Count() == 2)
                {
                    overhaul = nodeCodeSplitted[1];
                }

                markName.Append($".{nodeValue}");
            }
            if (subnodeCode != "-" && subnodeCode != "")
            {
                markName.Append($".{subnodeCode}");
                if (overhaul != "")
                {
                    markName.Append($"-{overhaul}");
                }
            }
            if (markCode != "-" && markCode != "")
            {
                markName.Append($"-{markCode}");
            }
            return markName.ToString();
        }

        public static (string, string) MakeComplexAndObjectName(
            string projectName,
            string nodeName,
            string subnodeName,
            string markName)
        {
            var complexName = projectName;
            var objectName = nodeName + ". " + subnodeName + ". " + markName;

            return (complexName, objectName);
        }
    }
}
