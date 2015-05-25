using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public static bool IsEnumerableType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IEnumerable));
        }

        public static bool IsCollectionType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(ICollection));
        }

        public static bool IsListType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IList));
        }

        public static bool IsListOrDictionaryType(this Type type)
        {
            return type.IsListType() || type.IsDictionaryType();
        }

        public static bool IsDictionaryType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                return true;

            var genericInterfaces = type.GetInterfaces().Where(t => t.IsGenericType);
            var baseDefinitions = genericInterfaces.Select(t => t.GetGenericTypeDefinition());
            return baseDefinitions.Any(t => t == typeof(IDictionary<,>));
        }

    }
}
