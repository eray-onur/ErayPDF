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
        private readonly IPrintStrategy _printStrategy;

        /// <summary>
        /// Static CTOR
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown if no webdrivers are available for conversion process.</exception>
        public HtmlToPdfConverter(IHostingEnvironment webEnvironment)
        {
            _pdfConversionOptions = new PdfConversionOptions();
            _webEnvironment = webEnvironment;

            // Check for any existing drivers.
            if (!File.Exists($"{_webEnvironment.ContentRootPath}/chromedriver.exe"))
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
            _printStrategy.Execute(_pdfConversionOptions, DetermineDriverPathForConversion(), htmlPath, ref pdfPath);

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
            _printStrategy.Execute(_pdfConversionOptions, DetermineDriverPathForConversion(), htmlPath, ref pdfPath);

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
            string pdfPath = string.Empty;
            _printStrategy.Execute(_pdfConversionOptions, DetermineDriverPathForConversion(), htmlPath, ref pdfPath);

            return pdfPath;
        }

        /// <summary>
        /// Traverses the project for any suitable webdriver.
        /// </summary>
        /// <returns>filepath for the found webdriver.</returns>
        private string DetermineDriverPathForConversion()
        {
            // TODO: Path determination logic.
            throw new NotImplementedException();
        }

    }
}
