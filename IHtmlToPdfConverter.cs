using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ErayPDF
{
    public interface IHtmlToPdfConverter
    {
        Task<byte[]> ConvertToBytes(string htmlPath);

        Task<MemoryStream> ConvertToMemoryStream(string htmlPath);

        string ConvertAndSavePDF(string htmlPath);
    }
}
