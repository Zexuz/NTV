using System;
using System.Collections.Generic;
using System.Web.Http;
using NTV.Core.Sites;
using NTV.Core.Sites.Reddit;

namespace NTV.WebApi.Controller.Controllers
{
    public class ImageController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<IDankResource> Get()
        {
            return new[]
            {
                new RedditResource
                {
                    Nsfw = false,
                    NumberOfComments = 550,
                    NumberOfLikes = 61458,
                    ResourceType = ResourceTypes.Image,
                    Title = "Sug min",
                    Uploaded = DateTime.Today,
                    UrlToSource = new Uri("https://www.reddit.com"),
                    UrlToWebsite = new Uri("https://www.imgut.com")
                },
                new RedditResource
                {
                    Nsfw = false,
                    NumberOfComments = 14,
                    NumberOfLikes = 550,
                    ResourceType = ResourceTypes.Gif,
                    Title = "trsaasd",
                    Uploaded = DateTime.Today,
                    UrlToSource = new Uri("https://www.reddit.com"),
                    UrlToWebsite = new Uri("https://www.imgut.com")
                },
                new RedditResource
                {
                    Nsfw = false,
                    NumberOfComments = 550,
                    NumberOfLikes = 487,
                    ResourceType = ResourceTypes.Image,
                    Title = "huiewgyu sbhjdfhjgsdghjfjghsdf",
                    Uploaded = DateTime.UtcNow,
                    UrlToSource = new Uri("https://www.reddit.com"),
                    UrlToWebsite = new Uri("https://www.imgut.com")
                }
            };
        }

        // GET api/<controller>/5
        public IDankResource Get(int id)
        {
            return new RedditResource
            {
                Nsfw = false,
                NumberOfComments = 550,
                NumberOfLikes = 61458,
                ResourceType = ResourceTypes.Image,
                Title = "Sug min",
                Uploaded = DateTime.Today,
                UrlToSource = new Uri("https://www.reddit.com"),
                UrlToWebsite = new Uri("https://www.imgut.com")
            };
        }
    }
}