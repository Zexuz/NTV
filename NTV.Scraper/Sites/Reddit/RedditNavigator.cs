using System;
using System.Linq;
using NTV.Scraper.Extensions;

namespace NTV.Scraper.Sites.Reddit
{
    public class RedditNavigator : IUriNavigator
    {
        public RedditJsonObject RedditJsonObject { get; set; }


        public Uri NextUri(Uri currentUri)
        {
            if (RedditJsonObject.data.children.Count == 0) return null;
            var lastChild = RedditJsonObject.data.children[RedditJsonObject.data.children.Count - 1];

            var lastChildId = $"{lastChild.kind}_{lastChild.data.id}";

            var uriString = $"{currentUri.GetFullUriAsString()}{currentUri.AbsolutePath}?limit=100&after={lastChildId}";
            return new Uri(uriString);
        }

        public Uri PrevUri(Uri currentUri)
        {
            throw new NotImplementedException();
        }

        public Uri AppenQueryStringToUri(Uri addPath)
        {
            return addPath.AddPath(".json?limit=100");
        }
    }
}