using OpenQA.Selenium;

using System;
using System.Collections.Generic;
using System.Text;

namespace ErayPDF.Models
{
    public class PdfConversionOptions
    {
        private readonly Dictionary<PageSize, PageDimensions> pageDimensions = new Dictionary<PageSize, PageDimensions>()
        {
            { PageSize.A5, new PageDimensions(15, 21) },
            { PageSize.A4, new PageDimensions(21, 29.7) },
            { PageSize.A3, new PageDimensions(29.7, 42) },
            { PageSize.A2, new PageDimensions(42, 59.4) },
            { PageSize.A1, new PageDimensions(59.4, 84.1) },
            { PageSize.Legal, new PageDimensions(22, 36) },
            { PageSize.Letter, new PageDimensions(22, 28) },
            { PageSize.Tabloid, new PageDimensions(27.9, 43.2) }
        };
        public PageDimensions selectedPageDimensions { get; set; }
        public double ScaleFactor { get; set; } = 1.0;
        public double PageWidth { get; set; }
        public double PageHeight { get; set; }
        public double MarginTop { get; set; } = 0;
        public double MarginBottom { get;set; } = 0;
        public double MarginLeft { get; set; } = 0;
        public double MarginRight { get; set; } = 0;

        public bool IncludeBackgroundGraphics { get; set; } = true;
        public bool IncludeHeaderAndFooter { get; set; } = true;
        public bool ShrinkToFit { get; set; } = true;
        public PrintOrientation OrientationType { get; set; } = PrintOrientation.Portrait;

        public PdfConversionOptions(PageSize pageSize = PageSize.A4)
        {
            selectedPageDimensions = pageDimensions.GetValueOrDefault(pageSize);

            PageWidth = selectedPageDimensions.Width;
            PageHeight = selectedPageDimensions.Width;
        }

        public PdfConversionOptions(PageSize pageSize, double scaleFactor, 
            double pageWidth, double pageHeight, 
            double marginT, double marginB, double marginL, double marginR, PrintOrientation orientationType)
        {
            if (pageDimensions.ContainsKey(pageSize))
            {
                selectedPageDimensions = pageDimensions.GetValueOrDefault(pageSize);
            }

            ScaleFactor = scaleFactor;
            PageWidth = pageWidth;
            PageHeight = pageHeight;
            MarginTop = marginT;
            MarginBottom = marginB;
            MarginLeft = marginL;
            MarginRight = marginR;

            OrientationType = orientationType;

        }

        public PdfConversionOptions(PageSize pageSize, double scaleFactor,
            double pageWidth, double pageHeight,
            double marginT, double marginB, double marginL, double marginR,
            bool includeBgGraphics, bool includeHeaderAndFooter, bool shrinkToFit, PrintOrientation orientationType)
        {
            if (pageDimensions.ContainsKey(pageSize))
            {
                selectedPageDimensions = pageDimensions.GetValueOrDefault(pageSize);
            }

            ScaleFactor = scaleFactor;
            PageWidth = pageWidth;
            PageHeight = pageHeight;
            MarginTop = marginT;
            MarginBottom = marginB;
            MarginLeft = marginL;
            MarginRight = marginR;

            IncludeBackgroundGraphics = includeBgGraphics;
            IncludeHeaderAndFooter = includeHeaderAndFooter;
            ShrinkToFit = shrinkToFit;

            OrientationType = orientationType;

        }
    }

    public struct PageDimensions
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public PageDimensions(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
