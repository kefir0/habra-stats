using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace HabrApi
{
    public static class Util
    {
        private static readonly Dictionary<char, string> Translit = new Dictionary<char, string>
                                                                        {
                                                                            {'а', "a"},
                                                                            {'б', "b"},
                                                                            {'в', "v"},
                                                                            {'г', "g"},
                                                                            {'д', "d"},
                                                                            {'е', "e"},
                                                                            {'ё', "e"},
                                                                            {'ж', "zh"},
                                                                            {'з', "z"},
                                                                            {'и', "i"},
                                                                            {'й', "y"},
                                                                            {'к', "k"},
                                                                            {'л', "l"},
                                                                            {'м', "m"},
                                                                            {'н', "n"},
                                                                            {'о', "o"},
                                                                            {'п', "p"},
                                                                            {'р', "r"},
                                                                            {'с', "s"},
                                                                            {'т', "t"},
                                                                            {'у', "u"},
                                                                            {'ф', "f"},
                                                                            {'х', "h"},
                                                                            {'ц', "c"},
                                                                            {'ч', "ch"},
                                                                            {'ш', "sh"},
                                                                            {'щ', "shh"},
                                                                            {'ъ', ""},
                                                                            {'ы', "i"},
                                                                            {'ь', ""},
                                                                            {'э', "e"},
                                                                            {'ю', "u"},
                                                                            {'я', "ya"}
                                                                        };

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

        public static string Transliterate(this string str)
        {
            // Borrowed here: http://www.koders.com/csharp/fidCA1D278AD6AD1BC6B2609D6B6F3060C08CE4B8E5.aspx?s=ftp
            // TODO: fix ucase/lcase
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var lowerCase = str[i].ToString().ToLower()[0];
                if (Translit.ContainsKey(lowerCase))
                {
                    var letter = Translit[lowerCase];
                    sb.Append(letter);
                }
                else
                {
                    sb.Append(str[i]);
                }
            }
            return sb.ToString();
        }

        public static string ToWebPageName(this string s)
        {
            return s.Transliterate().Replace(" ", "_");
        }

        public static DateTime ParseRusDateTime(string dateTimeString)
        {
            // "20 октября в 13:31" => "13:31 20 октября"
            // "сегодня" и "вчера" ещё
            var cultureInfo = CultureInfo.GetCultureInfo("ru-RU");
            dateTimeString = dateTimeString.Replace("сегодня", DateTime.Now.ToString("dd MMMM yyyy", cultureInfo));
            dateTimeString = dateTimeString.Replace("вчера", DateTime.Now.AddDays(-1).ToString("dd MMMM yyyy", cultureInfo));
            dateTimeString = string.Join(" ", dateTimeString.Split(new[] {" в "}, StringSplitOptions.RemoveEmptyEntries).Reverse());
            DateTime result;
            return DateTime.TryParse(dateTimeString, cultureInfo, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out result)
                       ? result
                       : DateTime.Today;
        }
    }
}