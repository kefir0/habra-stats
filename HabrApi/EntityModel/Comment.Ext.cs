﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HabrApi.EntityModel
{
    public partial class Comment
    {
        /*
            <a href="http://habrahabr.ru/users/Kobs/" class="avatar"><img src="http://habrahabr.ru/i/avatars/stub-user-small.gif" alt="" /></a>
            <a href="http://habrahabr.ru/users/Kobs/" class="username">Kobs</a> 
            <time datetime="2012-09-26T17:08:29+04:00">26 сентября 2012 в 17:08</time>
            <span class="score" title="Всего 74: &uarr;65 и &darr;9">+56</span>
         */

        private static readonly Regex CommentRegex = new Regex(
            "<div class=\"comment_item\" id=\"comment_(?<id>[0-9]+)\"" +
            ".*?" +
            "<span class=\"score\" title=\".*?&uarr;(?<plus>[0-9]+) и &darr;(?<minus>[0-9]+)\">" +
            ".*?" +
            "<a .*? class=\"avatar\"><img src=\"(?<avatar>.*?)\".*?/></a>" +
            ".*?" +
            "<a .*? class=\"username\">(?<user>.*?)</a>" +
            ".*?" +
            "<time.*?>(?<date>.*?)<" +
            ".*?" +
            "<div class=\"message.*?\">(?<text>.*?)</div>",
            RegexOptions.Singleline | RegexOptions.Compiled, TimeSpan.FromMilliseconds(200));

        public const string UrlFormat = "{0}#comment_{1}";

        public int ScoreMinus
        {
            get { return ScorePlus - Score; }
            set {} // For serialization
        }

        public string PostUrl
        {
            get { return Post.Url; }
            set { } // For serialization
        }

        public string PostTitle
        {
            get { return Post.Title; }
            set { } // For serialization
        }

        public string Url
        {
            get
            {
                // "http://habrahabr.ru/post/4035#comment_2"
                return string.Format(UrlFormat, PostUrl, Id);
            }
            set { } // For serialization
        }

        public static IEnumerable<Comment> Parse(string postHtml, Post post)
        {
            return CommentRegex.Matches(postHtml).OfType<Match>()
                .Select(c => MatchToComment(post, c));
        }

        private static Comment MatchToComment(Post post, Match c)
        {
            var scorePlus = int.Parse(c.Groups["plus"].Value);
            var scoreMinus = int.Parse(c.Groups["minus"].Value);
            var comment = new Comment
                              {
                                  Id = int.Parse(c.Groups["id"].Value),
                                  Score = scorePlus - scoreMinus,
                                  ScorePlus = scorePlus,
                                  Text = c.Groups["text"].Value.Trim(),
                                  PostId = post.Id,
                                  UserName = c.Groups["user"].Value,
                                  Avatar = c.Groups["avatar"].Value,
                                  Date = Util.ParseRusDateTime(c.Groups["date"].Value)
                              };
            return comment;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Score, Text);
        }
    }
}