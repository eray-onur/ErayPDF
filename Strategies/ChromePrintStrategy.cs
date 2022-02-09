using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;
using System.Collections.Generic;
using System.Text;

namespace ErayPDF.Strategies
{
    public class ChromePrintStrategy: IPrintStrategy
    {
        public void Execute(PdfConversionOptions options, string driverPath, string htmlPath, ref string pdfPath, string pdfName = "document")
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("headless");
            using ChromeDriver driver = new ChromeDriver(driverPath, chromeOptions);
            driver.Navigate().GoToUrl($"{Constants.LocalPrefix}{htmlPath}");
            PrintOptions opts = new PrintOptions()
            {
                ShrinkToFit = options.ShrinkToFit,
                OutputBackgroundImages = options.IncludeBackgroundGraphics,
                Orientation = options.OrientationType,
                ScaleFactor = options.ScaleFactor,
                PageDimensions =
                {
                    Width = 24.80,
                    Height = 35.08
                }

            };
            PrintDocument doc = driver.Print(opts);
            pdfPath = $"{AppContext.BaseDirectory}{pdfName}.{Constants.PdfSuffix}";
            doc.SaveAsFile(pdfPath);
        }
    }
}
