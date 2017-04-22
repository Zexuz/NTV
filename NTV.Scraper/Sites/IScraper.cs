using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTV.Scraper.Sites.Reddit;

namespace NTV.Scraper.Sites
{
    public interface IScraper
    {
        Uri BaseUri { get; }
        RateLimit RateLimit { get; }
        RedditNavigator Navigator { get; }
        Task<List<IDankResource>> GetResourcesFromSite();
        Enums.Sites GetType();
    }
}