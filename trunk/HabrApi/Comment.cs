using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HabrApi
{
    public class Comment
    {
        /*
			<a href="http://habrahabr.ru/users/Kobs/" class="avatar"><img src="http://habrahabr.ru/i/avatars/stub-user-small.gif" alt="" /></a>
			<a href="http://habrahabr.ru/users/Kobs/" class="username">Kobs</a>
         */

        private static readonly Regex CommentRegex = new Regex(
            "<div class=\"comment_item\" id=\"(?<id>.*?)\"" +
            ".*?" +
            "<span class=\"score\".*?>(?<score>.*?)</span>" +
            ".*?" +
            "<a .*? class=\"avatar\"><img src=\"(?<avatar>.*?)\".*?/></a>" +
            ".*?" +
            "<a .*? class=\"username\">(?<user>.*?)</a>" +
            ".*?" +
            "<div class=\"message.*?\">(?<text>.*?)</div>",
                                                              RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Id { get; set; }
        public int Score { get; set; }
        public int ScorePlus { get; set; }
        public int ScoreMinus { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string PostUrl { get; set; }
        public string PostTitle { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }

        public static IEnumerable<Comment> Parse(string postHtml, Post post)
        {
            return CommentRegex.Matches(postHtml).OfType<Match>()
                .Select(c =>
                        new Comment
                            {
                                Id = c.Groups["id"].Value,
                                Score = ParseCommentRating(c.Groups["score"].Value),
                                Text = c.Groups["text"].Value.Trim(),
                                Url = GetCommentUrl(post.Id, c.Groups["id"].Value),
                                PostUrl = post.Url,
                                PostTitle = post.Title,
                                UserName = c.Groups["user"].Value,
                                Avatar = c.Groups["avatar"].Value,
                            });
        }


        private static int ParseCommentRating(string commentRating)
        {
            return int.Parse(commentRating.Replace("–", "-"));
        }

        private static string GetCommentUrl(int postId, string commentId)
        {
            return string.Format(Post.UrlFormat, postId).TrimEnd('/') + "#" + commentId;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Score, Text);
        }
    }
}