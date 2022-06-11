using OpenQA.Selenium;

using System;
using System.Collections.Generic;
using System.Text;

namespace ErayPDF.Models
{
    public class PdfConversionOptions
    {
        private readonly Dictionary<string, PageDimensions> pageDimensions = new Dictionary<string, PageDimensions>()
        {
            { "A5", new PageDimensions(15, 21) },
            { "A4", new PageDimensions(21, 29.7) },
            { "A3", new PageDimensions(29.7, 42) },
            { "A2", new PageDimensions(42, 59.4) },
            { "A1", new PageDimensions(59.4, 84.1) },
            { "legal", new PageDimensions(22, 36) },
            { "letter", new PageDimensions(22, 28) },
            { "tabloid", new PageDimensions(27.9, 43.2) }
        };
        public PageDimensions selectedPageDimensions { get; set; }
        public double ScaleFactor { get; set; } = 0.0;
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

        public PdfConversionOptions()
        {
            selectedPageDimensions = pageDimensions.GetValueOrDefault("A4");
            PageWidth = selectedPageDimensions.Width;
            PageHeight = selectedPageDimensions.Width;
        }

        public PdfConversionOptions(string pageType)
        {
            if(pageDimensions.ContainsKey(pageType))
            {
                selectedPageDimensions = pageDimensions.GetValueOrDefault(pageType);
            }

        }

        public PdfConversionOptions(string pageType, double scaleFactor, 
            double pageWidth, double pageHeight, 
            double marginT, double marginB, double marginL, double marginR, PrintOrientation orientationType)
        {
            if (pageDimensions.ContainsKey(pageType))
            {
                selectedPageDimensions = pageDimensions.GetValueOrDefault(pageType);
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

        public PdfConversionOptions(string pageType, double scaleFactor,
            double pageWidth, double pageHeight,
            double marginT, double marginB, double marginL, double marginR,
            bool includeBgGraphics, bool includeHeaderAndFooter, bool shrinkToFit, PrintOrientation orientationType)
        {
            if (pageDimensions.ContainsKey(pageType))
            {
                selectedPageDimensions = pageDimensions.GetValueOrDefault(pageType);
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
