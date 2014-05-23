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
        // Все комбинации, напр "лучшие за год с картинками", "худшие короткие за всё время" итд

        [CommentReport(Category = "Содержимое", Name = "все", CategoryOrder = 2)]
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

        [CommentReport(Category = "Содержимое", Name = "короткие", CategoryOrder = 2)]
        public static IQueryable<Comment> Short(this IQueryable<Comment> comments)
        {
            return comments.Where(c => c.Text.Length < 33);
        }

        [CommentReport(Category = "Содержимое", Name = "длинные", CategoryOrder = 2)]
        public static IQueryable<Comment> Long(this IQueryable<Comment> comments)
        {
            return comments.Where(c => c.Text.Length > 1000);
        }

        [CommentReport(Category = "Содержимое", Name = "матерные", CategoryOrder = 2)]
        public static IQueryable<Comment> Obscene(this IQueryable<Comment> comments)
        {
            return comments.Where(c =>
                                  c.Text.Contains(" ебло") ||
                                  c.Text.Contains("аебал") ||
                                  c.Text.Contains("оебал") ||
                                  c.Text.Contains(" ебать") ||
                                  c.Text.Contains("оебать") ||
                                  c.Text.Contains("аебать") ||
                                  c.Text.Contains("ебало") ||
                                  c.Text.Contains("ебальн") ||
                                  c.Text.Contains("ёба") ||
                                  c.Text.Contains("ебаны") ||
                                  c.Text.Contains("заёб") ||
                                  c.Text.Contains("заеб") ||
                                  c.Text.Contains("разъёб") ||
                                  c.Text.Contains(" ебнул") ||
                                  c.Text.Contains("ебнут") ||
                                  c.Text.Contains("ёбн") ||
                                  c.Text.Contains("ебли") ||
                                  c.Text.Contains("бляд") ||
                                  c.Text.Contains(" блять") ||
                                  c.Text.Contains("блеа") ||
                                  c.Text.Contains("пизд") ||
                                  c.Text.Contains("хуй") ||
                                  c.Text.Contains("хуя") ||
                                  c.Text.Contains("хуё") ||
                                  c.Text.Contains("хуел") ||
                                  c.Text.Contains("хуев") ||
                                  c.Text.Contains("хуен") ||
                                  c.Text.Contains("пизд")
                );
        }

        //[CommentReport(Category = "Содержимое", Name = "топики зла", CategoryOrder = 2)]
        public static IQueryable<Comment> PostsOfEvil(this IQueryable<Comment> comments)
        {
            // Выбирает по одному худшему комментарию из каждого топика зла
            return comments
                .GroupBy(c => c.Post)
                .Where(g => g.Average(c => c.Score) < -3 && g.Count(c => c.Score > 0)*20 < g.Count())
                .OrderBy(g => g.Sum(c => c.Score))
                .Select(g => g.OrderBy(c => c.Score).FirstOrDefault())
                .Where(c => c != null);
        }

        //[CommentReport(Category = "Содержимое", Name = "топики добра", CategoryOrder = 2)]
        public static IQueryable<Comment> PostsOfGood(this IQueryable<Comment> comments)
        {
            // Выбирает по одному лучшему комментарию из каждого топика добра
            return comments
                .GroupBy(c => c.Post)
                .Where(g => g.Average(c => c.Score) > 10 && g.Count(c => c.Score < 0)*30 < g.Count())
                .OrderByDescending(g => g.Sum(c => c.Score))
                .Select(g => g.OrderByDescending(c => c.Score).FirstOrDefault())
                .Where(c => c != null);
        }

        [CommentReport(Category = "Рейтинг", Name = "лучшие", CategoryOrder = 1)]
        public static IQueryable<Comment> Best(this IQueryable<Comment> comments)
        {
            return comments.Where(c => c.Score > 10).OrderByDescending(c => c.Score);
        }

        [CommentReport(Category = "Рейтинг", Name = "худшие", CategoryOrder = 1)]
        public static IQueryable<Comment> Worst(this IQueryable<Comment> comments)
        {
            return comments.Where(c => c.Score < -10).OrderBy(c => c.Score);
        }

        //[CommentReport(Category = "Рейтинг", Name = "спорные", CategoryOrder = 1)]
        //public static IQueryable<Comment> Controversial(this IQueryable<Comment> comments)
        //{
        //    return comments.OrderBy(c => (c.ScorePlus - c.Score) * c.ScorePlus / (c.Score == 0 ? 1 : c.Score) + (c.ScorePlus + (c.ScorePlus - c.Score)) * 3);
        //}

        [CommentReport(Category = "Время", Name = "За сутки", CategoryOrder = 0)]
        public static IQueryable<Comment> Day(this IQueryable<Comment> comments)
        {
            var dateTime = DateTime.Now.AddDays(-1);
            return comments.Where(c => c.Date > dateTime);
        }

        [CommentReport(Category = "Время", Name = "За двое суток", CategoryOrder = 0)]
        public static IQueryable<Comment> TwoDays(this IQueryable<Comment> comments)
        {
            var dateTime = DateTime.Now.AddDays(-2);
            return comments.Where(c => c.Date > dateTime);
        }

        [CommentReport(Category = "Время", Name = "За неделю", CategoryOrder = 0)]
        public static IQueryable<Comment> Week(this IQueryable<Comment> comments)
        {
            var dateTime = DateTime.Now.AddDays(-7);
            return comments.Where(c => c.Date > dateTime);
        }

        [CommentReport(Category = "Время", Name = "За месяц", CategoryOrder = 0)]
        public static IQueryable<Comment> Month(this IQueryable<Comment> comments)
        {
            var dateTime = DateTime.Now.AddDays(-31);
            return comments.Where(c => c.Date > dateTime);
        }

        [CommentReport(Category = "Время", Name = "За год", CategoryOrder = 0)]
        public static IQueryable<Comment> Year(this IQueryable<Comment> comments)
        {
            var dateTime = DateTime.Now.AddDays(-365);
            return comments.Where(c => c.Date > dateTime);
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
            return GetAllCommentReportsEx().Select(r => new KeyValuePair<string, Func<IQueryable<Comment>, IQueryable<Comment>>>(
                                                            string.Join(" ", r.Key.Select(a => a.Name)), r.Value));
        }

        /// <summary>
        /// Generates list of all possible reports, in form of 'list of report attributes':'func to retrieve'.
        /// Does this by doing all possible combinations of report methods.
        /// </summary>
        public static IEnumerable<KeyValuePair<IEnumerable<CommentReportAttribute>, Func<IQueryable<Comment>, IQueryable<Comment>>>> GetAllCommentReportsEx()
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

        public static KeyValuePair<IEnumerable<CommentReportAttribute>, Func<IQueryable<Comment>, IQueryable<Comment>>> CombineMethods(IEnumerable<KeyValuePair<CommentReportAttribute, MethodInfo>> methodGroup)
        {
            var attributes = new List<CommentReportAttribute>();
            Func<IQueryable<Comment>, IQueryable<Comment>> resultFunc = null;
            foreach (var pair in methodGroup)
            {
                attributes.Add(pair.Key);
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

            return new KeyValuePair<IEnumerable<CommentReportAttribute>, Func<IQueryable<Comment>, IQueryable<Comment>>>(attributes, resultFunc);
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