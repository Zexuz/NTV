using System;
using NTV.Scraper.Enums;

namespace NTV.Runner
{
    public class SiteToScrape
    {
        public Sites SiteType { get; set; }
        public Uri Uri { get; set; }
        public bool Allow { get; set; }
    }
}