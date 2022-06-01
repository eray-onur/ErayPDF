using ErayPDF.Strategies;

using Microsoft.AspNetCore.Hosting;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ErayPDF
{
    public class HtmlToPdfConverter : IHtmlToPdfConverter
    {
        private readonly PdfConversionOptions _pdfConversionOptions;
        private readonly IHostingEnvironment _webEnvironment;
        private IPrintStrategy _printStrategy;
        private string driverPath => $"{_webEnvironment.ContentRootPath}/chromedriver.exe";

        /// <summary>
        /// Static CTOR
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown if no webdrivers are available for conversion process.</exception>
        public HtmlToPdfConverter(IHostingEnvironment webEnvironment)
        {
            _pdfConversionOptions = new PdfConversionOptions();
            _webEnvironment = webEnvironment;
            // Check for any existing drivers.
            if (!File.Exists(driverPath))
                throw new FileNotFoundException("Can't find any webdrivers.");
        }

        /// <summary>
        /// Returns an array of bytes that contain PDF data. Deletes the produced file after byte conversion.
        /// </summary>
        /// <param name="htmlPath">Path of html file that will be converted into PDF format.</param>
        /// <returns>Array of bytes that contain PDF data</returns>
        public async Task<byte[]> ConvertToBytes(string htmlPath)
        {
            string pdfPath = string.Empty;

            if (pdfPath == string.Empty)
                throw new FileNotFoundException();

            var bytes = await File.ReadAllBytesAsync(pdfPath);
            File.Delete(pdfPath);
            return bytes;
        }

        /// <summary>
        /// Returns a memory stream of PDF data. Deletes the produced PDF after memory stream.
        /// </summary>
        /// <param name="htmlPath">Path of html file that will be converted into PDF format.</param>
        /// <returns>Memory stream of PDF data</returns>
        public async Task<MemoryStream> ConvertToMemoryStream(string htmlPath)
        {

            string pdfPath = string.Empty;

            var memStream = new MemoryStream(await File.ReadAllBytesAsync(pdfPath), false);
            File.Delete(pdfPath);

            return memStream;
        }

        /// <summary>
        /// Persisting converted PDF file and returning its path. Persists the PDF.
        /// </summary>
        /// <param name="htmlPath"></param>
        /// <returns></returns>
        public string ConvertAndSavePDF(string htmlPath)
        {
            string pdfPath = "example";
            //_printStrategy.Execute(_pdfConversionOptions, DetermineDriverPathForConversion(), htmlPath, ref pdfPath);

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("headless");
            using ChromeDriver driver = new ChromeDriver($"C:\\Projects\\ErayPDF\\ErayPDF\\chromium-102.0.5005.63-6_Win64", chromeOptions);
            driver.Navigate().GoToUrl($"{Constants.LocalPrefix}{htmlPath}");

            PdfConversionOptions options = new PdfConversionOptions
            {
                ShrinkToFit = true,
                IncludeBackgroundGraphics = true,
                OrientationType = PrintOrientation.Portrait,
                ScaleFactor = 1.0,
                MarginTop = 0,
                MarginBottom = 0,
                MarginLeft = 0,
                MarginRight = 0,
            };

            var margins = new PrintOptions.Margins();
            margins.Top = options.MarginTop;
            margins.Bottom = options.MarginBottom;
            margins.Left = options.MarginLeft;
            margins.Right = options.MarginRight;

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
            opts.PageMargins.Top = margins.Top;
            opts.PageMargins.Bottom = margins.Bottom;
            opts.PageMargins.Left = margins.Left;
            opts.PageMargins.Right = margins.Right;

            PrintDocument doc = driver.Print(opts);
            pdfPath = $"{AppContext.BaseDirectory}{pdfPath}.{Constants.PdfSuffix}";
            doc.SaveAsFile(pdfPath);

            return pdfPath;
        }

    }
}
