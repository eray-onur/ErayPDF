using System;
using System.Collections.Generic;
using System.Text;

namespace ErayPDF.Strategies
{
    public interface IPrintStrategy
    {
        void Execute(PdfConversionOptions options, string driverPath, string htmlPath, ref string pdfPath, string pdfName = "document");
    }
}
