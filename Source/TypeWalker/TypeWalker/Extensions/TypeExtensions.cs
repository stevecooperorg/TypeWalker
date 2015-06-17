using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsExportableType(Type t)
        {
            var n = t.Name;

            return
                    //!t.IsValueType &&
                    !t.IsEnum &&
                    !(t.Assembly.FullName == typeof(string).Assembly.FullName) &&
                    !(t.Assembly.FullName == typeof(object).Assembly.FullName) &&
                    !(t == typeof(object)) &&
                    !t.IsGenericTypeDefinition &&
                    !t.ContainsGenericParameters &&
                    !t.Name.Contains("<") &&
                    !t.IsGenericType;
            
        }

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public static bool IsDictionaryType(this Type type)
        {
            var acceptableTypes = new[] {
                typeof(IDictionary<,>),
                typeof(Dictionary<,>),    
                typeof(KeyValuePair<,>),
            };
            Type collectionOf;
            return IsGenericType(type, acceptableTypes, out collectionOf);
        }



        public static bool IsGenericCollectionType(this Type type, out Type collectionOf)
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

            return IsGenericType(type, acceptableTypes, out collectionOf);
        }


        private static bool IsGenericType(this Type type, Type[] acceptableTypes, out Type collectionOf)
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

        public static bool IsArrayType(this Type type)
        {
            return type.IsArray;
        }

    }
}
