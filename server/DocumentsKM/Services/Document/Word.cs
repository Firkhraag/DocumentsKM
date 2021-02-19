using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocumentsKM.Services
{
    public static class Word
    {
        public static Run GetTextElement(
            string text,
            int fSize,
            bool isUnderlined = false,
            bool isSuperscript = false)
        {
            Run run = new Run();
            RunProperties runProperties = run.AppendChild(new RunProperties());
            Italic italic = new Italic();
            italic.Val = OnOffValue.FromBoolean(true);
            FontSize fontSize = new FontSize() { Val = fSize.ToString() };
            runProperties.AppendChild(italic);
            runProperties.AppendChild(fontSize);
            RunFonts font = new RunFonts()
            {
                Ascii = "GOST type B",
                HighAnsi = "GOST type B",
                ComplexScript = "GOST type B"
            };
            runProperties.Append(font);
            if (isUnderlined)
            {
                Underline underline = new Underline()
                {
                    Val = UnderlineValues.Single,
                };
                runProperties.Append(underline);
            }
            if (isSuperscript)
            {
                VerticalTextAlignment vertAlign = new VerticalTextAlignment()
                {
                    Val = VerticalPositionValues.Superscript,
                };
                runProperties.Append(vertAlign);
            }
            run.AppendChild(new Text()
            {
                Text = text,
                Space = SpaceProcessingModeValues.Preserve,
            });
            return run;
        }
    }
}
