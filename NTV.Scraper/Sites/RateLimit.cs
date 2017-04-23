using System;

namespace NTV.Scraper.Sites
{
    public class RateLimit
    {
        private readonly TimeSpan _scraperDelay;

        public RateLimit(TimeSpan scraperDelay)
        {
            _scraperDelay = scraperDelay;
        }

        public TimeSpan GetTimeSpanUntillNextScrape()
        {
            return _scraperDelay;
        }
    }
}