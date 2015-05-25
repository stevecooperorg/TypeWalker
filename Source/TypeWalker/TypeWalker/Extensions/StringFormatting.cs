using System.Text;
using System.Text.RegularExpressions;

namespace TypeWalker.Extensions
{
    public static class StringFormatting
    {
        public static StringBuilder AppendFormatObject(this StringBuilder sb, string format, object o)
        {
            sb.Append(format.ㄍ(o));
            return sb;
        }

        private static string GetStringFrom(object o, string propertyName)
        {
            var property = o.GetType().GetProperty(propertyName);
            var value = property.GetValue(o, null);
            var stringForm = value == null ? "" : value.ToString();
            return stringForm;
        }

        public static string ㄍ(this string format, object o)
        {
            var rx = new Regex(@"\{(?<name>.*?)\}");
            var result = rx.Replace(format, me => GetStringFrom(o, me.Groups["name"].Value));

            result = result.Replace(@"{{", @"{");
            result = result.Replace(@"}}", @"}");

            return result;
        }
    }
}