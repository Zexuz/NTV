using System.Collections.Generic;
using System.Threading.Tasks;
using NTV.Scraper.Sites;
using NTV.Scraper.Sites.Reddit;

namespace NTV.Scraper.Services
{
    public class ScraperService
    {
        private readonly HttpRequestExecutorService _httpRequestExecutorService;
        public ScraperService(HttpRequestExecutorService httpRequestExecutorService)
        {
            _httpRequestExecutorService = httpRequestExecutorService;
        }

        public List<IDankResource> ScrapeReddit()
        {
            var httpExecutor = _httpRequestExecutorService;
            var redditSite = new Reddit(httpExecutor);

            var task = redditSite.GetResourcesFromSite();
            Task.WaitAll(task);
            return task.Result;
        }

    }

}