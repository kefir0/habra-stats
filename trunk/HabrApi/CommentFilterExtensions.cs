using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HabrApi.EntityModel;

namespace HabrApi
{
    public static class CommentFilterExtensions
    {
        // TODO: Все комбинации, напр "лучшие за год с картинками", "худшие короткие за всё время" итд (методы с атрибутами и комбинации!)

        [CommentReport(Category = "Содержимое", Name = "С картинкой")]
        public static IQueryable<Comment> WithPicture(this IQueryable<Comment> comments)
        {
            return comments.OrderByDescending(c => c.Score)
                .Where(c => c.Text.Contains(".jpg") || c.Text.Contains(".png") || c.Text.Contains(".gif"));
        }

        [CommentReport(Category = "Содержимое", Name = "Со ссылкой")]
        public static IQueryable<Comment> WithLink(this IQueryable<Comment> comments)
        {
            return comments.OrderByDescending(c => c.Score)
                .Where(c => c.Text.Contains("http://"));
        }

        [CommentReport(Category = "Размер", Name = "Короткие")]
        public static IQueryable<Comment> Short(this IQueryable<Comment> comments)
        {
            return comments.Where(c => c.Text.Length < 13);
        }

        [CommentReport(Category = "Размер", Name = "Длинные")]
        public static IQueryable<Comment> Long(this IQueryable<Comment> comments)
        {
            return comments.Where(c => c.Text.Length > 1000);
        }

        [CommentReport(Category = "Рейтинг", Name = "Лучшие")]
        public static IQueryable<Comment> Best(this IQueryable<Comment> comments)
        {
            return comments.OrderByDescending(c => c.Score);
        }

        [CommentReport(Category = "Рейтинг", Name = "Худшие")]
        public static IQueryable<Comment> Worst(this IQueryable<Comment> comments)
        {
            return comments.OrderBy(c => c.Score);
        }

        [CommentReport(Category = "Рейтинг", Name = "Спорные")]
        public static IQueryable<Comment> Controversial(this IQueryable<Comment> comments)
        {
            return comments.OrderBy(c => c.ScoreMinus*c.ScorePlus/(c.Score == 0 ? 1 : c.Score) + (c.ScorePlus + c.ScoreMinus)*3);
        }

        [CommentReport(Category = "Время", Name = "за сутки")]
        public static IQueryable<Comment> Day(this IQueryable<Comment> comments)
        {
            return comments.Where(c => (DateTime.Now - c.Date).TotalDays < 1);
        }

        [CommentReport(Category = "Время", Name = "за неделю")]
        public static IQueryable<Comment> Week(this IQueryable<Comment> comments)
        {
            return comments.Where(c => (DateTime.Now - c.Date).TotalDays < 7);
        }

        [CommentReport(Category = "Время", Name = "за месяц")]
        public static IQueryable<Comment> Month(this IQueryable<Comment> comments)
        {
            return comments.Where(c => (DateTime.Now - c.Date).TotalDays < 31);
        }

        [CommentReport(Category = "Время", Name = "за год")]
        public static IQueryable<Comment> Year(this IQueryable<Comment> comments)
        {
            return comments.Where(c => (DateTime.Now - c.Date).TotalDays < 365);
        }

        [CommentReport(Category = "Время", Name = "за всё время")]
        public static IQueryable<Comment> AllTime(this IQueryable<Comment> comments)
        {
            return comments;
        }

        public static IEnumerable<KeyValuePair<string, IQueryable<Comment>>> GetAllCommentReports()
        {
            var groups = GetCommentReportMethods().GroupBy(m => m.Key.Category);
            var combinations = GetAllCombinations(groups, Func<>);
            return null;
        }

        public static KeyValuePair<string, IQueryable<Comment>> CombineReports(KeyValuePair<string, IQueryable<Comment>> x, KeyValuePair<string, IQueryable<Comment>> y)
        {
            return new KeyValuePair<string, IQueryable<Comment>>(string.Format("{0} - {1}", x, y));
        }

        public static IEnumerable<T>  GetAllCombinations<T, TCombination>(IEnumerable<IEnumerable<T>> groups, Func<T, T, TCombination> combine)
        {
            // Take first element, combine
        }

        public static IEnumerable<KeyValuePair<CommentReportAttribute, MethodInfo>> GetCommentReportMethods()
        {
            foreach (var methodInfo in typeof (CommentFilterExtensions).GetMethods())
            {
                var attr = methodInfo.GetCustomAttribute<CommentReportAttribute>();
                if (attr != null)
                {
                    yield return new KeyValuePair<CommentReportAttribute, MethodInfo>(attr, methodInfo);
                }
            }
        }
    }
}