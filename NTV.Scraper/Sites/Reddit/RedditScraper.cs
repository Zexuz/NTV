﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NTV.Core.Sites;
using NTV.Core.Sites.Reddit;
using NTV.Scraper.Extensions;

namespace NTV.Scraper.Sites.Reddit
{
    public class RedditScraper : IScraper
    {
        public Uri BaseUri => new Uri("https://www.reddit.com");
        public RateLimit RateLimit { get; }
        public RedditNavigator Navigator { get; }
        public List<string> PathsToScrape => ToScrape();
        public List<string> PathsToAvoid => AvoidScrape();

        private readonly IHttpRequestExecutor _httpExecutor;

        public RedditScraper(IHttpRequestExecutor httpExecutor)
        {
            _httpExecutor = httpExecutor;
            RateLimit = new RateLimit(TimeSpan.FromSeconds(2));
            Navigator = new RedditNavigator();
        }

        public async Task<List<IDankResource>> GetResourcesFromSite()
        {
            var list = new List<IDankResource>();
            foreach (var path in PathsToScrape)
            {
                var nextUri = Navigator.AppenQueryStringToUri(BaseUri.AddPath(path));

                while(true)
                {
                    list.AddRange(await ScrapeUri(nextUri));
                    //sleep untill we should scrape again.
                    var sleepTime = RateLimit.GetTimeSpanUntillNextScrape().TotalMilliseconds;
                    Console.WriteLine($"Seleeping for {sleepTime}ms");
                    await Task.Delay((int)sleepTime);

                    nextUri = Navigator.NextUri(nextUri);
                    if (nextUri== null) break;
                }
            }
            return list;

        }

        private async Task<IEnumerable<IDankResource>> ScrapeUri(Uri uri)
        {
            Console.WriteLine($"Scraping uri {uri.AbsoluteUri}");
            var response = await _httpExecutor.GetResponseFromSite(uri);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Can't get response from site:{response.Headers.Location.AbsoluteUri}");
            }

            var dataString = await response.Content.ReadAsStringAsync();
            return  ConvertDataString(dataString);
        }


        public new Core.Enums.Sites GetType()
        {
            return Core.Enums.Sites.Reddit;
        }


        private IEnumerable<IDankResource> ConvertDataString(string dataString)
        {
            var rootObject = JsonConvert.DeserializeObject<RedditJsonObject>(dataString);
            Navigator.RedditJsonObject = rootObject;

            return rootObject.data.children
                .Where(ResourceMatchesCriteria)
                .Select(child => new RedditResource
                {
                    NumberOfComments = child.data.num_comments,
                    NumberOfLikes = child.data.score,
                    Uploaded = UnixTimeStampToDateTime((int) child.data.created_utc),
                    UrlToSource = new Uri(child.data.url),
                    UrlToWebsite = new Uri($"{BaseUri.GetLeftPart(UriPartial.Authority)}{child.data.permalink}"),
                    Nsfw = child.data.over_18,
                    Title = child.data.title,
                    ResourceType = GetResourceTypeFromChild(child)
                })
                .Cast<IDankResource>()
                .ToList();
        }

        private ResourceTypes GetResourceTypeFromChild(Child child)
        {
            if (new Regex(@".+(gif|gifv)$").IsMatch(child.data.url)) return ResourceTypes.Gif;
            if (new Regex(@".+(jpeg|jpg|png)$").IsMatch(child.data.url)) return ResourceTypes.Image;
            if(child.data.post_hint == "rich:video") return ResourceTypes.Video;
            if(child.data.post_hint.ToLower() == "image") return ResourceTypes.Image;

            throw new ArgumentException("Child is not a valid Resource type");

        }

        private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private static bool ResourceMatchesCriteria(Child child)
        {
            if (AvoidScrape().Any(path => string.Equals(child.data.subreddit_name_prefixed, path, StringComparison.CurrentCultureIgnoreCase)))
            {
                return false;
            }

            if (child.data.stickied) return false;
            if (child.data.score < 500) return false;
            if (child.data.post_hint == "image" || child.data.post_hint == "rich:video") return true;
            if (new Regex(@".+(jpeg|jpg|gif|png|gifv)$").IsMatch(child.data.url)) return true;

            return false;
        }


        private static List<string> ToScrape()
        {
            return new List<string>
            {
                "top/",
                "r/funny/",
                "top/",
                "r/funny/",
                "r/pics/",
                "r/dankmemes/",
                "r/videos/",
                "domain/youtube.com/",
                "domain/imgur.com/"
            };
        }

        private static List<string> AvoidScrape()
        {
            return new List<string>
            {
                "r/The_Donald",
                "r/politics"
            };
        }
    }
}