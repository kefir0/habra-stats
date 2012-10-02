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
            "<div class=\"message.*?\">(?<text>.*?)</div>",
            RegexOptions.Singleline | RegexOptions.Compiled);

        public static IEnumerable<Comment> Parse(string postHtml, Post post)
        {
            return CommentRegex.Matches(postHtml).OfType<Match>()
                .Select(c =>
                        new Comment
                            {
                                Id = int.Parse(c.Groups["id"].Value),
                                ScorePlus = int.Parse(c.Groups["plus"].Value),
                                ScoreMinus = int.Parse(c.Groups["minus"].Value),
                                Text = c.Groups["text"].Value.Trim(),
                                Url = GetCommentUrl(post.Id, c.Groups["id"].Value),
                                PostUrl = post.Url,
                                PostTitle = post.Title,
                                UserName = c.Groups["user"].Value,
                                Avatar = c.Groups["avatar"].Value,
                            });
        }

        partial void OnScoreMinusChanged()
        {
            Score = ScorePlus - ScoreMinus;
        }

        partial void OnScorePlusChanged()
        {
            Score = ScorePlus - ScoreMinus;
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