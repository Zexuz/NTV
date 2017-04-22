using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NTV.Scraper
{
    public interface IHttpRequestExecutor
    {
        Task<HttpResponseMessage> GetResponseFromSite(Uri uri);
    }
}