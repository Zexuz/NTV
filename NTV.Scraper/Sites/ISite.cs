using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTV.Scraper.Sites
{
    public interface ISite
    {
        Uri BaseUri { get; }
        Task<List<IDankResource>> GetResourcesFromSite();
        Enums.Sites GetType();
    }
}