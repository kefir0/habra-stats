using System;
using System.Collections.Generic;

namespace HabrApi.EntityModel
{
    /// <summary>
    ///     Represents a site, such as Habrahabr, Geektimes, Megamozg
    /// </summary>
    public class Site
    {
        public const string UrlFormat = "{0}/post/{1}";

        public static readonly IReadOnlyCollection<Site> Instances = new List<Site>
        {
            new Site(0, "http://habrahabr.ru"),
            new Site(1, "http://geektimes.ru"),
            new Site(2, "http://megamozg.ru"),
        }.AsReadOnly();

        private readonly int _id;
        private readonly string _url;

        private Site(int id, string url)
        {
            _id = id;
            _url = url;
        }

        public int Id
        {
            get { return _id; }
        }

        public string Url
        {
            get { return _url; }
        }

        public string GetUrl(int postId)
        {
            return string.Format(UrlFormat, Url, postId);
        }
    }
}