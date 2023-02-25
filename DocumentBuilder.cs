using ErayPDF.Models;

using Microsoft.AspNetCore.Hosting;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ErayPDF
{
    public class DocumentBuilder
    {
        // Directory path name of the chrome webdriver.
        private string _chromiumRootPath => Path.Join(AppContext.BaseDirectory, "Binaries");

        // Chrome webdriver executable name.
        private string _driverName => "chromedriver.exe";
        // Path to the webdriver for chrome.
        private string _driverFullPath => Path.Join(findValidChromiumByOS(), _driverName);

        private PdfConversionOptions _convOptions;
        private PrintOptions _printOptions;

        // Where HTML files to be processed reside.
        private string _htmlPathForPrint = Path.Join(AppContext.BaseDirectory, "ProcessedFiles", "HTML");
        // Where converted PDF files reside.
        private string _pdfPathForPrint = Path.Join(AppContext.BaseDirectory, "ProcessedFiles", "PDF");

        private List<PrintableFileInformation> _htmlFilesForPrint = new List<PrintableFileInformation>();

        /// <summary>
        /// Determining the path to chromium by hosted OS.
        /// </summary>
        /// <returns></returns>
        private string findValidChromiumByOS()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Path.Join(_chromiumRootPath, "Win64");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Path.Join(_chromiumRootPath, "Linux");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return Path.Join(_chromiumRootPath, "OSX");
            }

            throw new InvalidOperationException("Current operating system is not supported.");
        }

        public DocumentBuilder(PdfConversionOptions options = null)
        {
            if (!File.Exists(_driverFullPath))
                throw new FileNotFoundException("Can't find any webdrivers.");

            if (!Directory.Exists(_htmlPathForPrint))
                Directory.CreateDirectory(_htmlPathForPrint);

            if (!Directory.Exists(_pdfPathForPrint))
                Directory.CreateDirectory(_pdfPathForPrint);



            #region [Default Config Setup]
            if(options == null)
            {
                _convOptions = new PdfConversionOptions
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
            }
            else
            {
                _convOptions = options;
            }

            #endregion

            _printOptions = new PrintOptions()
            {
                ShrinkToFit = _convOptions.ShrinkToFit,
                OutputBackgroundImages = _convOptions.IncludeBackgroundGraphics,
                Orientation = _convOptions.OrientationType,
                ScaleFactor = _convOptions.ScaleFactor,
                PageDimensions = { Width = _convOptions.selectedPageDimensions.Width, Height = _convOptions.selectedPageDimensions.Height }
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
        /// Creating an HTML file from plain string for printing.
        /// </summary>
        /// <param name="htmlContent">Stringified HTML content.</param>
        /// <param name="htmlName">Name of the HTML file. Defaults to a random UUID.</param>
        /// <param name="shouldPersistHtmlFile">Whether or not the created HTML file should be persistent. Defaults to false.</param>
        /// <returns></returns>
        public async Task<DocumentBuilder> FromHtmlContent(string htmlContent, bool shouldPersistHtmlFile = false, string htmlName = null)
        {
            if (htmlName == null)
                htmlName = Guid.NewGuid().ToString();

            var path = Path.Join(_htmlPathForPrint, htmlName);

            await File.WriteAllTextAsync(path , htmlContent);

            _htmlFilesForPrint.Add(new PrintableFileInformation(path, shouldPersistHtmlFile));

            return this;
        }

        /// <summary>
        /// Adding HTML via path to be processed.
        /// </summary>
        /// <param name="htmlPath"></param>
        /// <returns></returns>
        public DocumentBuilder FromFilePath(string htmlPath)
        {
            _htmlFilesForPrint.Add(new PrintableFileInformation(htmlPath, true));
            return this;
        }

        /// <summary>
        /// Returns the printed PDF files' path.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public string AsFilePath(string pdfName)
        {
            PrintDocument doc = PrintAsPdf();
            string pdfPath = Path.Join(_pdfPathForPrint, string.Concat(pdfName, ".", Constants.PdfSuffix));
            doc.SaveAsFile(pdfPath);

            Cleanup();

            return pdfPath;
        }

        /// <summary>
        /// Returns the printed PDFs in binary format.
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        public byte[] AsBinary()
        {
            PrintDocument doc = PrintAsPdf();

            Cleanup();
            
            var bytes = doc.AsByteArray;

            return bytes;
        }

        public string AsBase64String()
        {
            PrintDocument doc = PrintAsPdf();

            Cleanup();

            string base64str = doc.AsBase64EncodedString;

            return base64str;
        }

        private void Cleanup()
        {
            foreach(var fileInfo in _htmlFilesForPrint)
            {
                if (fileInfo.ShouldPersist == false)
                {
                    File.Delete(fileInfo.HtmlFilePath);
                }
            }
            _htmlFilesForPrint.Clear();
        }

        private PrintDocument PrintAsPdf()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.BinaryLocation = _chromiumRootPath;
            
            chromeOptions.AddArgument("headless");
            using ChromeDriver driver = new ChromeDriver(findValidChromiumByOS(), chromeOptions);
            string foundHtmlPath = _htmlFilesForPrint[0].HtmlFilePath;

            var urlPath = Path.Join(Constants.LocalPrefix, foundHtmlPath);
            driver.Navigate().GoToUrl(urlPath);
            
            PrintDocument doc = driver.Print(_printOptions);    

            return doc;
        }

    }
}
