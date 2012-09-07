using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace HabraStatsService.Habra
{
    internal class Habr
    {
        private void Delme()
        {
            var wc = new WebClient() { Encoding = Encoding.UTF8 };

            Func<string, string> GetCachePath = s =>
            {
                var cachePath = @"c:\temp\HabrCache";
                var fileName = s.Replace("/", "-").Replace(".", "-").Replace(":", "-");
                fileName = Path.Combine(cachePath, fileName + ".html");
                return fileName;
            };

            Func<string, string> Download = s =>
            {
                var fileName = GetCachePath(s);
                if (File.Exists(fileName))
                {
                    return File.ReadAllText(fileName);
                }
                else
                {
                    var html = "";
                    try
                    {
                        html = wc.DownloadString(s);
                    }
                    catch { }
                    File.WriteAllText(fileName, html);
                    return html;
                }
            };

            Func<string, int> ParseCommentRating = s =>
            {
                //("|" + s.Trim() + "|").Dump();
                //((int)s[0]).Dump();
                return int.Parse(s.Replace("–", "-"));
            };

            const string postUrlFormat = "http://habrahabr.ru/post/{0}/";
            var lastPostHtml = Download("http://habrahabr.ru/posts/collective/new/");
            var lastPostRegex = new Regex(string.Format(postUrlFormat, "([0-9]+)"));
            var match = lastPostRegex.Match(lastPostHtml);
            var lastPostId = int.Parse(match.Groups[1].Value);
            var allComments = new List<dynamic>();

            //var commentRegex = new Regex("<div class=\"comment item\".*?<div class=\"mark.*?<div class=\"message.*?\">(.*?)<//div>");
            var commentRegex = new Regex("<div class=\"comment_item\" id=\"(.*?)\".*?<span class=\"score\".*?>(.*?)</span>.*?<div class=\"message.*?\">(.*?)</div>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Take some recent posts and parse comments
            for (var id = lastPostId; id > lastPostId - 500; id--)
            {
                var postUrl = string.Format(postUrlFormat, id);
                var postHtml = Download(postUrl);
                //postHtml.Dump();
                var comments = commentRegex.Matches(postHtml).OfType<Match>().Select(c =>
                new
                {
                    Id = c.Groups[1].Value,
                    Score = ParseCommentRating(c.Groups[2].Value),
                    Text = c.Groups[3].Value.Trim(),
                    Url = Util.RawHtml(string.Format("<a href='{0}'>{0}</a>", postUrl.TrimEnd('/') + "#" + c.Groups[1].Value))
                });
                allComments.AddRange(comments);
            }

            allComments.OrderByDescending(c => c.Score).Take(20).Dump();
            allComments.OrderBy(c => c.Score).Take(20).Dump();
            
        }
    }
}