using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErayPDF
{
    public class HtmlToPdfConverter
    {
        class Constants
        {
            public const string LocalPrefix = "file:///";
            public const string HttpPrefix = "http://";
            public const string HttpsPrefix = "https://";
            public const string PdfSuffix = "pdf";
        }

        /// <summary>
        /// Static CTOR
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown if no webdrivers are available for conversion process.</exception>
        static HtmlToPdfConverter()
        {
            if (!File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/chromedriver.exe"))
                throw new FileNotFoundException("Can't find any webdrivers.");
        }

        /// <summary>
        /// Returns an array of bytes that contain PDF data.
        /// </summary>
        /// <param name="htmlPath">Path of html file that will be converted into PDF format.</param>
        /// <returns>Array of bytes that contain PDF data</returns>
        public async static Task<byte[]> ConvertToBytes(string htmlPath)
        {
            var path = ConvertHtmlToPdf(htmlPath);
            var bytes = await File.ReadAllBytesAsync(path);
            //File.Delete(path);
            return bytes;
        }

        /// <summary>
        /// Returns a memory stream of PDF data.
        /// </summary>
        /// <param name="htmlPath">Path of html file that will be converted into PDF format.</param>
        /// <returns>Memory stream of PDF data</returns>
        public async static Task<MemoryStream> ConvertToMemoryStream(string htmlPath)
        {
            var path = ConvertHtmlToPdf(htmlPath);
            var memStream = new MemoryStream(await File.ReadAllBytesAsync(path), false);
            File.Delete(path);
            return memStream;
        }

        /// <summary>
        /// Persisting converted PDF file and returning its path.
        /// </summary>
        /// <param name="htmlPath"></param>
        /// <returns></returns>
        public static string ConvertAndSavePDF(string htmlPath)
        {

            var path = ConvertHtmlToPdf(htmlPath);
            return path;
        }

        /// <summary>
        /// Converting a provided HTML file into PDF.
        /// </summary>
        /// <param name="htmlPath">Path of html file that will be converted into PDF format.</param>
        /// <param name="docName">Name of the PDF file to be created.</param>
        /// <returns>Path of created PDF file</returns>
        /// <exception cref="FileNotFoundException">Thrown if HTML file is not found in given location.</exception>
        private static string ConvertHtmlToPdf(string htmlPath, string docName = "doc")
        {
            if (!File.Exists(htmlPath))
                throw new FileNotFoundException();

            docName = docName.Replace(".pdf", "");
            string? path = AppDomain.CurrentDomain.BaseDirectory;

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("headless");
            using ChromeDriver driver = new(path, chromeOptions);
            driver.Navigate().GoToUrl($"{Constants.LocalPrefix}{htmlPath}");

            // driver.ExecuteAsyncScript("window.print();");
            PrintOptions opts = new()
            {
                ShrinkToFit = true,
                OutputBackgroundImages = true,
                Orientation = PrintOrientation.Portrait,
                //ScaleFactor = 0.5,
                PageDimensions =
                {
                    Width = 24.80,
                    Height = 35.08
                }

            };

            PrintDocument doc = driver.Print(opts);
            var pdfPath = $"{path}{docName}.{Constants.PdfSuffix}";
            doc.SaveAsFile(pdfPath);
            return pdfPath;
        }

    }
}
