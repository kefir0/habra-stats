using System;
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
            "<time datetime=\"(?<date>.*?)\">" +
            ".*?" +
            "<div class=\"message.*?\">(?<text>.*?)</div>",
            RegexOptions.Singleline | RegexOptions.Compiled);

        public int ScoreMinus
        {
            get { return Score - ScorePlus; }
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
                                  Url = GetCommentUrl(post.Id, c.Groups["id"].Value),
                                  PostId = post.Id,
                                  UserName = c.Groups["user"].Value,
                                  Avatar = c.Groups["avatar"].Value,
                                  Date = DateTime.Parse(c.Groups["date"].Value)
                              };
            return comment;
        }

        private static string GetCommentUrl(int postId, string commentId)
        {
            return string.Format(Post.UrlFormat, postId).TrimEnd('/') + "#comment_" + commentId;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Score, Text);
        }
    }
}