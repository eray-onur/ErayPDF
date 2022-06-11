using System;
using System.Collections.Generic;
using System.Text;

namespace ErayPDF.Models
{
    public class PrintableFileInformation
    {
        public string HtmlFilePath { get; }
        public bool ShouldPersist { get; }

        public PrintableFileInformation(string htmlFilePath, bool shouldPersist)
        {
            HtmlFilePath = htmlFilePath;
            ShouldPersist = shouldPersist;
        }
    }
}
