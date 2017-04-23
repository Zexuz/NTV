using System;

namespace NTV.Core.Sites.Reddit
{
    public class RedditResource:IDankResource
    {
        public Uri UrlToSource { get; set; }
        public Uri UrlToWebsite { get; set; }
        public DateTime Uploaded { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public bool Nsfw { get; set; }
        public string Title { get; set; }
        public ResourceTypes ResourceType { get; set; }
    }
}