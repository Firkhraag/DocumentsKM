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
            string markName,
            int bias)
        {
            var complexName = projectName;
            var objectName = "";
            var firstPartAdded = false;
            if (nodeName != "" && nodeName != null)
            {
                objectName += nodeName;
                firstPartAdded = true;
            }
            if (subnodeName != "" && subnodeName != null)
            {
                if (firstPartAdded)
                    objectName += ". ";
                else
                    firstPartAdded = true;
                objectName += subnodeName;
            }
            if (markName != "" && markName != null)
            {
                if (firstPartAdded)
                    objectName += ". ";
                objectName += markName;
            }
            if (bias > 0)
            {
                complexName = projectName + ". " + objectName.Substring(0, bias -2);
                objectName = objectName.Substring(bias, objectName.Length);
            }
            else if (bias < 0)
            {
                complexName = projectName.Substring(0, projectName.Length + bias - 2);
                objectName = projectName.Substring(projectName.Length + bias) + ". " + objectName;
            }

            return (complexName, objectName);
        }
    }
}
