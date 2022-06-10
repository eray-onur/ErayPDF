using Microsoft.AspNetCore.Hosting;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ErayPDF
{
    public class DocumentBuilder
    {
        // Directory path name of the chrome webdriver.
        private string driverRootPath => $"{AppContext.BaseDirectory}/Binaries/Win64";
        // Chrome webdriver executable name.
        private string driverName => "chromedriver.exe";
        // Path to the webdriver for chrome.
        private string driverFullPath => Path.Join(driverRootPath, driverName);

        private PdfConversionOptions _convOptions;
        private PrintOptions _printOptions;

        // Where HTML files to be processed reside.
        private string _htmlPathForPrint = Path.Join(AppContext.BaseDirectory, "ProcessedFiles", "HTML");
        // Where converted PDF files reside.
        private string _pdfPathForPrint = Path.Join(AppContext.BaseDirectory, "ProcessedFiles", "PDF");
        private List<string> _htmlFilesForPrint = new List<string>();

        public DocumentBuilder()
        {
            if (!File.Exists(driverFullPath))
                throw new FileNotFoundException("Can't find any webdrivers.");

            if (!Directory.Exists(_htmlPathForPrint))
                Directory.CreateDirectory(_htmlPathForPrint);

            if (!Directory.Exists(_pdfPathForPrint))
                Directory.CreateDirectory(_pdfPathForPrint);


            #region [Default Config Setup]
            _convOptions = new PdfConversionOptions
            {
                ShrinkToFit = true,
                IncludeBackgroundGraphics = true,
                OrientationType = PrintOrientation.Portrait,
                ScaleFactor = 1.0,
                MarginTop = 1,
                MarginBottom = 1,
                MarginLeft = 1,
                MarginRight = 1,
            };

            _printOptions = new PrintOptions()
            {
                ShrinkToFit = _convOptions.ShrinkToFit,
                OutputBackgroundImages = _convOptions.IncludeBackgroundGraphics,
                Orientation = _convOptions.OrientationType,
                ScaleFactor = _convOptions.ScaleFactor,

                PageDimensions =
                {
                    Width = 21.0,
                    Height = 29.7
                }

            };


            var margins = new PrintOptions.Margins();

            margins.Top = _convOptions.MarginTop;
            margins.Bottom = _convOptions.MarginBottom;
            margins.Left = _convOptions.MarginLeft;
            margins.Right = _convOptions.MarginRight;

            _printOptions.PageMargins.Top = margins.Top;
            _printOptions.PageMargins.Bottom = margins.Bottom;
            _printOptions.PageMargins.Left = margins.Left;
            _printOptions.PageMargins.Right = margins.Right;

            #endregion

        }

        public void ResetPagesToPrint()
        {
            _htmlFilesForPrint.Clear();
        }

        public DocumentBuilder WithConversionOptions(PdfConversionOptions options)
        {
            _convOptions = options;
            return this;
        }

        public DocumentBuilder WithPrintingOptions(PrintOptions options)
        {
            _printOptions = options;
            return this;
        }

        /// <summary>
        /// Adding HTML via stringified content to be processed.
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public async Task<DocumentBuilder> FromHtmlContent(string htmlContent)
        {
            await File.WriteAllTextAsync(_htmlPathForPrint, htmlContent);
            return this;
        }

        /// <summary>
        /// Adding HTML via path to be processed.
        /// </summary>
        /// <param name="htmlPath"></param>
        /// <returns></returns>
        public DocumentBuilder FromFilePath(string htmlPath)
        {
            _htmlFilesForPrint.Add(htmlPath);
            return this;
        }

        /// <summary>
        /// Returns the printed PDF files' path.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public string AsFilePath(string pdfName)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("headless");
            using ChromeDriver driver = new ChromeDriver(driverRootPath, chromeOptions);

            var urlPath = Path.Join(Constants.LocalPrefix, _htmlFilesForPrint[0]);
            driver.Navigate().GoToUrl(urlPath);

            PrintDocument doc = driver.Print(_printOptions);
            string pdfPath = Path.Join(_pdfPathForPrint, string.Concat(pdfName, ".", Constants.PdfSuffix));
            doc.SaveAsFile(pdfPath);

            ResetPagesToPrint();

            return pdfPath;
        }

        /// <summary>
        /// Returns the printed PDFs in binary format.
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        public async Task<byte[]> AsBinary(string pdfName)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("headless");
            using ChromeDriver driver = new ChromeDriver(driverRootPath, chromeOptions);

            var urlPath = Path.Join(Constants.LocalPrefix, _htmlFilesForPrint[0]);
            driver.Navigate().GoToUrl(urlPath);

            PrintDocument doc = driver.Print(_printOptions);
            string pdfPath = Path.Join(_pdfPathForPrint, pdfName, Constants.PdfSuffix);
            doc.SaveAsFile(pdfPath);

            ResetPagesToPrint();

            return await File.ReadAllBytesAsync(pdfPath);
        }
    }
}
