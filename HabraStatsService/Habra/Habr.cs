﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace HabraStatsService.Habra
{
    public class Habr
    {
        private const string PostUrlFormat = "http://habrahabr.ru/post/{0}/";
        private const string RecentPostsUrl = "http://habrahabr.ru/posts/collective/new/";
        private const string CachePath = @"e:\HabrCache";

        public string DownloadString(string url)
        {
            var fileName = GetCachePath(url);
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            
            var html = "";
            var wc = new WebClient { Encoding = Encoding.UTF8 };
            try
            {
                html = wc.DownloadString(url);
            }
            catch { }

            File.WriteAllText(fileName, html);
            return html;
        }

        public string DownloadPost(int postId)
        {
            return DownloadString(string.Format(PostUrlFormat, postId));
        }

        private static string GetCachePath(string postUrl)
        {
            var fileName = postUrl.Replace("/", "-").Replace(".", "-").Replace(":", "-");
            fileName = Path.Combine(CachePath, fileName + ".html");
            return fileName;
        }

        private static int ParseCommentRating(string commentRating)
        {
            return int.Parse(commentRating.Replace("–", "-"));
        }

        public IEnumerable<Post> GetRecentPosts(int postCount)
        {
            var lastPostHtml = DownloadString(RecentPostsUrl);
            var lastPostRegex = new Regex(string.Format(PostUrlFormat, "([0-9]+)"));
            var match = lastPostRegex.Match(lastPostHtml);
            var lastPostId = int.Parse(match.Groups[1].Value);
            var commentRegex = new Regex("<div class=\"comment_item\" id=\"(.*?)\".*?<span class=\"score\".*?>(.*?)</span>.*?<div class=\"message.*?\">(.*?)</div>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            for (var i = lastPostId; i > lastPostId - postCount; i--)
            {
                var postHtml = DownloadPost(i);
                var comments = commentRegex.Matches(postHtml).OfType<Match>()
                    .Select(c =>
                            new Comment
                                {
                                    Id = c.Groups[1].Value,
                                    Score = ParseCommentRating(c.Groups[2].Value),
                                    Text = c.Groups[3].Value.Trim(),
                                    Url = GetCommentUrl(i, c.Groups[1].Value)
                                });
                yield return new Post {Id = i, Comments = comments.ToArray()};
            }
        }

        private static string GetCommentUrl(int postId, string commentId)
        {
            return string.Format(PostUrlFormat, postId).TrimEnd('/') + "#" + commentId;
        }
    }
}