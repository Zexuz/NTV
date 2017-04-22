using System;

namespace NTV.Scraper.Sites
{
    public interface IDankResource
    {
        Uri UrlToSource { get; set; }
        Uri UrlToWebsite { get; set; }
        DateTime Uploaded { get; set; }
        int NumberOfLikes { get; set; }
        int NumberOfComments { get; set; }
        bool Nsfw { get; set; }
        string Title { get; set; }
    }
}