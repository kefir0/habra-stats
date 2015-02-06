﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HabrApi.EntityModel
{
    public partial class Post
    {
        public const string UrlFormat = "{0}/post/{1}";
        private static readonly Regex TitleRegex = new Regex("<title>(.*) / .*</title>", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex DateRegex = new Regex("<div class=\"published\">(.*?)</div>", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex ScoreRegex = new Regex("<span class=\"score\" .*?>(.*?)</span>", RegexOptions.Singleline | RegexOptions.Compiled);
        private Site _site = Site.Instances.First();

        public string Url
        {
            get { return GetUrl(Id, Site); }
        }

        public double DaysOld
        {
            get { return (DateTime.Now - Date).TotalDays; }
        }

        public Site Site
        {
            get { return _site; }
            private set { _site = value; }
        }

        public static string GetUrl(int postId)
        {
            return GetUrl(postId, Site.Instances.First());
        }

        public static string GetUrl(int postId, Site site)
        {
            if (site == null)
            {
                throw new ArgumentNullException("site");
            }
            return String.Format(UrlFormat, site.Url, postId);
        }

        public static IEnumerable<string> GetUrlVariants(int postId)
        {
            return Site.Instances.Select(x => GetUrl(postId, x));
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1}", Id, Title);
        }

        public static Post Parse(string html, int id, bool skipComments = false, bool includeZeroVoteComments = false)
        {
            if (string.IsNullOrEmpty(html))
                return null;

            try
            {
                var title = TitleRegex.Match(html).Groups[1].Value;
                int score;
                if (!int.TryParse(ScoreRegex.Match(html).Groups[1].Value, out  score))
                    score = 0;
                var dateMatch = DateRegex.Match(html);
                if (!dateMatch.Success)
                    return null;

                var dateTimeString = dateMatch.Groups[1].Value;
                var date = Util.ParseRusDateTime(dateTimeString);
                var post = new Post
                               {
                                   Id = id,
                                   Title = title,
                                   Date = date,
                                   Score = score
                               };

                if (!skipComments)
                {
                    foreach (var comment in Comment.Parse(html, post)
                        .Where(comment => includeZeroVoteComments || comment.Score > 0 || comment.ScorePlus > 0))
                    {
                        post.Comments.Add(comment);
                    }
                }

                return post;
            }
            catch (Exception)
            {
                return null;
            }
        }

        partial void OnSiteIdChanged()
        {
            Site = Site.Instances.First(x => x.Id == SiteId);
        }

        partial void OnSiteIdChanging(int value)
        {
            if (!Site.Instances.Any(x => x.Id == value))
            {
                throw new ArgumentException("Invalid SiteId: " + value);
            }
        }
    }
}