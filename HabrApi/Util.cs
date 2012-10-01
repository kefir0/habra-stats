using System.IO;
using System.Runtime.Serialization;
using System.Text;
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

        public static string ToXmlD(this object obj)
        {
            var s = new DataContractSerializer(obj.GetType());
            using (var writer = new MemoryStream())
            {
                s.WriteObject(writer, obj);
                return Encoding.UTF8.GetString(writer.GetBuffer());
            }
        }
    }
}