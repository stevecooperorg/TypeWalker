using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeWalker.Extensions
{
    /// <summary>
    /// Extensions to help make judgements about Types
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Determines if the given type is exportable
        /// </summary>
        /// <param name="t"></param>
        /// <returns>
        /// True if the type is exportable; otherwise, false
        /// </returns>
        /// <remarks>
        /// A type is exportable if none of the following are true:
        /// <list type="bullet">
        /// T is Enum
        /// T is String
        /// T is Object
        /// T is Generic Type Definition (ie, non-concrete)
        /// T is Generic (ie, has generic type arguments)
        /// </list>
        /// </remarks>
        public static bool IsExportableType(this Type t)
        {
            return !t.IsEnum &&
                    !(t.Assembly.FullName == typeof(string).Assembly.FullName) &&
                    !(t.Assembly.FullName == typeof(object).Assembly.FullName) &&
                    !(t == typeof(object)) &&
                    !t.IsGenericTypeDefinition &&
                    !t.ContainsGenericParameters &&
                    !t.Name.Contains("<") &&
                    !t.IsGenericType;

        }

        /// <summary>
        /// Determines if the given type is derived from System.Nullable
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// True if the given type is derived from System.Nullable; otherwise, false
        /// </returns>
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && (typeof(Nullable<>) == type.GetGenericTypeDefinition());
        }

        public static bool IsDictionaryType(this Type type)
        {
            var acceptableTypes = new[] {
                typeof(IDictionary<,>),
                typeof(Dictionary<,>),    
                typeof(KeyValuePair<,>),
            };
            return IsGenericType(type, acceptableTypes);
        }

        /// <summary>
        /// Checks whether the given type is a collection, and if it is,
        /// tries to get the type of the collection
        /// </summary>
        /// <param name="type"></param>
        /// <param name="collectionOf">
        /// The Type of the collection, or null if type is not a collection Type
        /// </param>
        /// <returns>
        /// True if the collection type was able to be found; otherwise, false
        /// </returns>
        public static bool TryGetGenericCollectionType(this Type type, out Type collectionOf)
        {
            if (type.IsArrayType())
            {
                collectionOf = type.GetElementType();
                return true;
            }

            var acceptableTypes = new[] {
                typeof(ICollection<>),
                typeof(IEnumerable<>),
                typeof(IList<>),
                typeof(List<>),
                typeof(System.Collections.ObjectModel.ReadOnlyCollection<>),
            };

            return TryGetGenericType(type, acceptableTypes, out collectionOf);
        }

        public static bool IsGenericCollectionType(this Type type)
        {
            Type throwAway;
            return TryGetGenericCollectionType(type, out throwAway);
        }

        private static bool TryGetGenericType(this Type type, Type[] acceptableTypes, out Type collectionOf)
        {
            if (!type.IsGenericType)
            {
                collectionOf = null;
                return false;
            }

            var genericType = type.GetGenericTypeDefinition();
            if (acceptableTypes.Contains(genericType))
            {
                collectionOf = type.GetGenericArguments()[0];
                return true;
            }

            collectionOf = null;
            return false;
        }

        private static bool IsGenericType(this Type type, Type[] acceptableTypes)
        {
            Type throwAway;
            return TryGetGenericType(type, acceptableTypes, out throwAway);
        }

        public static bool IsArrayType(this Type type)
        {
            return type.IsArray;
        }
    }
}