using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HabrApi
{
    public class Comment
    {
        private static readonly Regex CommentRegex = new Regex("<div class=\"comment_item\" id=\"(.*?)\".*?<span class=\"score\".*?>(.*?)</span>.*?<div class=\"message.*?\">(.*?)</div>",
                                                              RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Id { get; set; }
        public int Score { get; set; }
        public int ScorePlus { get; set; }
        public int ScoreMinus { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string PostUrl { get; set; }
        public string PostTitle { get; set; }

        public static IEnumerable<Comment> Parse(string postHtml, Post post)
        {
            return CommentRegex.Matches(postHtml).OfType<Match>()
                .Select(c =>
                        new Comment
                            {
                                Id = c.Groups[1].Value,
                                Score = ParseCommentRating(c.Groups[2].Value),
                                Text = c.Groups[3].Value.Trim(),
                                Url = GetCommentUrl(post.Id, c.Groups[1].Value),
                                PostUrl = post.Url,
                                PostTitle = post.Title
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