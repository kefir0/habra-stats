using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace HabrApi
{
    public class StatsGenerator
    {
        private static XslCompiledTransform _commentsXslt;

        private static XslCompiledTransform GetCommentsXslt()
        {
            if (_commentsXslt == null)
            {
                var executingAssembly = Assembly.GetExecutingAssembly();
                _commentsXslt = new XslCompiledTransform();
                using (var resourceStream = executingAssembly.GetManifestResourceStream("HabrApi.CommentStats.xslt"))
                {
                    if (resourceStream == null)
                        throw new Exception("Cannot load comments transform");
                    _commentsXslt.Load(XmlReader.Create(resourceStream));
                }
            }
            return _commentsXslt;
        }

        public string GenerateCommentStats(IEnumerable<Post> posts)
        {
            var comments = posts.SelectMany(p => p.Comments).OrderByDescending(c => c.Score)
                .Take(100).ToArray();

            return TransformData(comments, GetCommentsXslt());
        }

        private static string TransformData(object data, XslCompiledTransform transform)
        {
            var serializer = new XmlSerializer(data.GetType());
            using (var sourceStream = new MemoryStream())
            using (var sourceWriter = new XmlTextWriter(sourceStream, Encoding.UTF8))
            using (var resultStream = new MemoryStream())
            using (var resultWriter = new XmlTextWriter(resultStream, Encoding.UTF8))
            {
                serializer.Serialize(sourceWriter, data);
                sourceStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new XmlTextReader(sourceStream))
                {
                    transform.Transform(reader, resultWriter);
                    return Encoding.UTF8.GetString(resultStream.GetBuffer());
                }
            }
        }
    }
}