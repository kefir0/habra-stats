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

        [CommentReport(Category = "Содержимое", Name = "", CategoryOrder = 2)]
        public static IQueryable<Comment> NoFilter(this IQueryable<Comment> comments)
        {
            return comments;
        }

        [CommentReport(Category = "Содержимое", Name = "с картинкой", CategoryOrder = 2)]
        public static IQueryable<Comment> WithPicture(this IQueryable<Comment> comments)
        {
            return comments.OrderByDescending(c => c.Score)
                .Where(c => c.Text.Contains(".jpg") || c.Text.Contains(".png") || c.Text.Contains(".gif"));
        }

        [CommentReport(Category = "Содержимое", Name = "со ссылкой", CategoryOrder = 2)]
        public static IQueryable<Comment> WithLink(this IQueryable<Comment> comments)
        {
            return comments.OrderByDescending(c => c.Score)
                .Where(c => c.Text.Contains("http://"));
        }

        [CommentReport(Category = "Содержимое", Name = "короткие", CategoryOrder = 2)]
        public static IQueryable<Comment> Short(this IQueryable<Comment> comments)
        {
            return comments.Where(c => c.Text.Length < 13);
        }

        [CommentReport(Category = "Содержимое", Name = "длинные", CategoryOrder = 2)]
        public static IQueryable<Comment> Long(this IQueryable<Comment> comments)
        {
            return comments.Where(c => c.Text.Length > 1000);
        }

        [CommentReport(Category = "Рейтинг", Name = "лучшие", CategoryOrder = 1)]
        public static IQueryable<Comment> Best(this IQueryable<Comment> comments)
        {
            return comments.OrderByDescending(c => c.Score);
        }

        [CommentReport(Category = "Рейтинг", Name = "худшие", CategoryOrder = 1)]
        public static IQueryable<Comment> Worst(this IQueryable<Comment> comments)
        {
            return comments.OrderBy(c => c.Score);
        }

        [CommentReport(Category = "Рейтинг", Name = "спорные", CategoryOrder = 1)]
        public static IQueryable<Comment> Controversial(this IQueryable<Comment> comments)
        {
            return comments.OrderBy(c => c.ScoreMinus*c.ScorePlus/(c.Score == 0 ? 1 : c.Score) + (c.ScorePlus + c.ScoreMinus)*3);
        }

        [CommentReport(Category = "Время", Name = "За сутки", CategoryOrder = 0)]
        public static IQueryable<Comment> Day(this IQueryable<Comment> comments)
        {
            return comments.Where(c => (DateTime.Now - c.Date).TotalDays < 1);
        }

        [CommentReport(Category = "Время", Name = "За неделю", CategoryOrder = 0)]
        public static IQueryable<Comment> Week(this IQueryable<Comment> comments)
        {
            return comments.Where(c => (DateTime.Now - c.Date).TotalDays < 7);
        }

        [CommentReport(Category = "Время", Name = "За месяц", CategoryOrder = 0)]
        public static IQueryable<Comment> Month(this IQueryable<Comment> comments)
        {
            return comments.Where(c => (DateTime.Now - c.Date).TotalDays < 31);
        }

        [CommentReport(Category = "Время", Name = "За год", CategoryOrder = 0)]
        public static IQueryable<Comment> Year(this IQueryable<Comment> comments)
        {
            return comments.Where(c => (DateTime.Now - c.Date).TotalDays < 365);
        }

        [CommentReport(Category = "Время", Name = "За всё время", CategoryOrder = 0)]
        public static IQueryable<Comment> AllTime(this IQueryable<Comment> comments)
        {
            return comments;
        }

        /// <summary>
        /// Generates list of all possible reports, in form of 'report name':'func to retrieve'.
        /// Does this by doing all possible combinations of report methods.
        /// </summary>
        public static IEnumerable<KeyValuePair<string, Func<IQueryable<Comment>, IQueryable<Comment>>>> GetAllCommentReports()
        {
            // T = IEnumerable<KeyValuePair<CommentReportAttribute, MethodInfo>>
            var groups = GetCommentReportMethods()
                .OrderBy(m => m.Key.CategoryOrder)
                .GroupBy(m => m.Key.Category)
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