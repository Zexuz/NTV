using System;

namespace NTV.Scraper.Sites
{
    public interface IUriNavigator
    {
        Uri NextUri(Uri currentUri);
        Uri PrevUri(Uri currentUri);
    }
}