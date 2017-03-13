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

        public static string ㄍ(this string format, object o)
        {
            var rx = new Regex(@"\{(?<name>.*?)\}");
            var result = rx.Replace(format, me => GetStringFrom(o, me.Groups["name"].Value));

            result = result.Replace(@"{{", @"{");
            result = result.Replace(@"}}", @"}");

            return result;
        }

        private static string GetStringFrom(object o, string propertyName)
        {
            var dotIndex = propertyName.IndexOf(".");
            if (dotIndex != -1)
            {
                // looks like 'foo.bar', so go get the foo object first...
                var leftMost = propertyName.Substring(0, dotIndex);
                var rightMost = propertyName.Substring(dotIndex + 1);
                var objectProperty = o.GetType().GetProperty(leftMost).GetValue(o, null);
                return GetStringFrom(objectProperty, rightMost);
            }

            var property = o.GetType().GetProperty(propertyName);
            var value = property.GetValue(o, null);
            var stringForm = value == null ? "" : value.ToString();
            return stringForm;
        }
    }
}