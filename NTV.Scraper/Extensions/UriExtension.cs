using System;

namespace NTV.Scraper.Extensions
{
    public static class UriExtension
    {
        public static string GetFullUriAsString(this Uri uri)
        {
            return uri.GetLeftPart(UriPartial.Authority);
        }


        public static Uri AddPath(this Uri uri, string path)
        {
            return new Uri(new Uri(uri.GetFullUriAsString() + uri.AbsolutePath), path);
        }
    }
}