using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public static IEnumerable<KeyValuePair<string, Func<IQueryable<Comment>, IQueryable<Comment>>>> GetAllCommentReports()
        {
            // T = IEnumerable<KeyValuePair<CommentReportAttribute, MethodInfo>>
            var groups = GetCommentReportMethods().GroupBy(m => m.Key.Category)
                .Select(g => g.Select(p => p.ToEnumerable()));
            var methodGroups = GetAllCombinations(groups, (g1, g2) => g1.Concat(g2));
            return methodGroups.Select(CombineMethods);
        }

        public static IEnumerable<T> ToEnumerable<T>(this T obj)
        {
            yield return obj;
        }

        public static KeyValuePair<string, Func<IQueryable<Comment>, IQueryable<Comment>>> CombineMethods(IEnumerable<KeyValuePair<CommentReportAttribute, MethodInfo>> methodGroup)
        {
            var sb = new StringBuilder();
            Func<IQueryable<Comment>, IQueryable<Comment>> resultFunc = null;
            foreach (var pair in methodGroup)
            {
                sb.Append(pair.Key.Name).Append(' ');
                var func = (Func<IQueryable<Comment>, IQueryable<Comment>>) Delegate.CreateDelegate(typeof (Func<IQueryable<Comment>, IQueryable<Comment>>), pair.Value);
                if (resultFunc == null)
                {
                    resultFunc = func;
                }
                else
                {
                    var f1 = resultFunc; // Do not access modified closure
                    resultFunc = x => f1(func(x));
                }
            }

            return new KeyValuePair<string, Func<IQueryable<Comment>, IQueryable<Comment>>>(sb.ToString().Trim(), resultFunc);
        }

        public static IEnumerable<T> GetAllCombinations<T>(IEnumerable<IEnumerable<T>> groups, Func<T, T, T> combine)
        {
            return groups.Aggregate((g1, g2) => g1.SelectMany(g => g2.Select(gg2 => combine(g, gg2))));
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