using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NTV.Scraper
{
    public class HttpRequestExecutorService : IHttpRequestExecutor
    {
        public async Task<HttpResponseMessage> GetResponseFromSite(Uri uri)
        {
            var httpClient = new HttpClient();
            return await httpClient.GetAsync(uri);
        }
    }
}