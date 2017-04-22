using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NTV.Scraper.Sites.Reddit
{
    public class Reddit : ISite
    {
        public Uri BaseUri => new Uri("https://www.reddit.com");

        private readonly List<string> _pathsToScrape = new List<string>
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

        private readonly List<string> _pathsToAvoid = new List<string>
        {
            "r/The_Donald",
            "r/politics"
        };


        private readonly IHttpRequestExecutor _httpExecutor;

        public Reddit(IHttpRequestExecutor httpExecutor)
        {
            _httpExecutor = httpExecutor;
        }

        public async Task<List<IDankResource>> GetResourcesFromSite()
        {
            var list = new List<IDankResource>();
            foreach (var path in _pathsToScrape)
            {
                var uri = new Uri(BaseUri, $"{path}.json?limit=100");
                Console.WriteLine($"Scraping {uri.AbsolutePath}");
                var response = await _httpExecutor.GetResponseFromSite(uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Can't get response from site:{response.Headers.Location.AbsoluteUri}");
                }

                var dataString = await response.Content.ReadAsStringAsync();
                list.AddRange(ConvertDataString(dataString));
            }

            return list;
        }

        private IEnumerable<IDankResource> ConvertDataString(string dataString)
        {
            var rootObject = JsonConvert.DeserializeObject<RootObject>(dataString);

            return rootObject.data.children
                .Where(child => !child.data.stickied && !ShouldAvoidSubredit(child.data.subreddit_name_prefixed))
                .Select(child => new RedditResource
                {
                    NumberOfComments = child.data.num_comments,
                    NumberOfLikes = child.data.score,
                    Uploaded = UnixTimeStampToDateTime((int) child.data.created_utc),
                    UrlToSource = new Uri(child.data.url),
                    UrlToWebsite = new Uri($"{BaseUri.GetLeftPart(UriPartial.Authority)}{child.data.permalink}"),
                    Nsfw = child.data.over_18,
                    Title = child.data.title
                })
                .Cast<IDankResource>()
                .ToList();
        }

        private bool ShouldAvoidSubredit(string subredit)
        {
            return _pathsToAvoid.Any(path => path.ToLower().Equals(subredit.ToLower()));
        }

        public new Enums.Sites GetType()
        {
            return Enums.Sites.Reddit;
        }

        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

    #region RedditJsonClass

    public class MediaEmbed
    {
    }

    public class SecureMediaEmbed
    {
    }

    public class Source
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Resolution
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Source2
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Resolution2
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Gif
    {
        public Source2 source { get; set; }
        public List<Resolution2> resolutions { get; set; }
    }

    public class Source3
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Resolution3
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Mp4
    {
        public Source3 source { get; set; }
        public List<Resolution3> resolutions { get; set; }
    }

    public class Source4
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Resolution4
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Obfuscated
    {
        public Source4 source { get; set; }
        public List<Resolution4> resolutions { get; set; }
    }

    public class Source5
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Resolution5
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Nsfw
    {
        public Source5 source { get; set; }
        public List<Resolution5> resolutions { get; set; }
    }

    public class Variants
    {
        public Gif gif { get; set; }
        public Mp4 mp4 { get; set; }
        public Obfuscated obfuscated { get; set; }
        public Nsfw nsfw { get; set; }
    }

    public class Image
    {
        public Source source { get; set; }
        public List<Resolution> resolutions { get; set; }
        public Variants variants { get; set; }
        public string id { get; set; }
    }

    public class Preview
    {
        public List<Image> images { get; set; }
        public bool enabled { get; set; }
    }

    public class Data2
    {
        public bool contest_mode { get; set; }
        public object banned_by { get; set; }
        public MediaEmbed media_embed { get; set; }
        public string subreddit { get; set; }
        public object selftext_html { get; set; }
        public string selftext { get; set; }
        public object likes { get; set; }
        public object suggested_sort { get; set; }
        public List<object> user_reports { get; set; }
        public object secure_media { get; set; }
        public string link_flair_text { get; set; }
        public string id { get; set; }
        public int gilded { get; set; }
        public SecureMediaEmbed secure_media_embed { get; set; }
        public bool clicked { get; set; }
        public int score { get; set; }
        public object report_reasons { get; set; }
        public string author { get; set; }
        public bool saved { get; set; }
        public List<object> mod_reports { get; set; }
        public string name { get; set; }
        public string subreddit_name_prefixed { get; set; }
        public object approved_by { get; set; }
        public bool over_18 { get; set; }
        public string domain { get; set; }
        public bool hidden { get; set; }
        public Preview preview { get; set; }
        public string thumbnail { get; set; }
        public string subreddit_id { get; set; }
        public bool edited { get; set; }
        public string link_flair_css_class { get; set; }
        public string author_flair_css_class { get; set; }
        public int downs { get; set; }
        public bool brand_safe { get; set; }
        public bool archived { get; set; }
        public object removal_reason { get; set; }
        public string post_hint { get; set; }
        public bool is_self { get; set; }
        public bool hide_score { get; set; }
        public bool spoiler { get; set; }
        public string permalink { get; set; }
        public object num_reports { get; set; }
        public bool locked { get; set; }
        public bool stickied { get; set; }
        public double created { get; set; }
        public string url { get; set; }
        public string author_flair_text { get; set; }
        public bool quarantine { get; set; }
        public string title { get; set; }
        public double created_utc { get; set; }
        public object distinguished { get; set; }
        public object media { get; set; }
        public int num_comments { get; set; }
        public bool visited { get; set; }
        public string subreddit_type { get; set; }
        public int ups { get; set; }
        public bool? author_cakeday { get; set; }
    }

    public class Child
    {
        public string kind { get; set; }
        public Data2 data { get; set; }
    }

    public class Data
    {
        public string modhash { get; set; }
        public List<Child> children { get; set; }
        public string after { get; set; }
        public object before { get; set; }
    }

    public class RootObject
    {
        public string kind { get; set; }
        public Data data { get; set; }
    }

    #endregion
}