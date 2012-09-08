using System.IO;
using System.Xml.Serialization;

namespace HabrApi
{
    public static class Util
    {
        public static string ToXml(this object obj)
        {
            var s = new XmlSerializer(obj.GetType());
            using (var writer = new StringWriter())
            {
                s.Serialize(writer, obj);
                return writer.ToString();
            }
        }
    }
}