using System;
using System.Linq;
using NTV.Scraper;
using NTV.Scraper.Services;

namespace NTV.Runner
{
    internal class Program
    {



        public static void Main(string[] args)
        {

            var screaperService = new ScraperService(new HttpRequestExecutorService());


            var data = screaperService.ScrapeReddit();

            foreach (var dankResource in data)
            {
                Console.WriteLine(dankResource.UrlToWebsite);
            }
        }


    }
}