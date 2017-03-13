using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeWalker.Extensions;
using TypeWalker.Generators;

namespace TypeWalker
{
    public class Visitor
    {
        private Language language;

        private Queue<Type> toVisit = new Queue<Type>();

        private HashSet<Type> visited = new HashSet<Type>();

        public event EventHandler<MemberEventArgs> MemberVisited;

        public event EventHandler<MemberEventArgs> MemberVisiting;

        public event EventHandler<MethodEventArgs> MethodVisited;

        public event EventHandler<MethodEventArgs> MethodVisiting;

        public event EventHandler<NameSpaceEventArgs> NameSpaceVisited;

        public event EventHandler<NameSpaceEventArgs> NameSpaceVisiting;

        public event EventHandler<TypeEventArgs> TypeVisited;

        public event EventHandler<TypeEventArgs> TypeVisiting;

        public void Visit(IEnumerable<Type> types, Language language)
        {
            this.language = language;

            foreach (var type in types)
            {
                this.RegisterType(type);
            }

            while (this.toVisit.Count > 0)
            {
                var type = this.toVisit.Dequeue();
                if (!this.visited.Contains(type))
                {
                    VisitType(type);
                }
            }
        }

        private void AddIfMissing(Type type)
        {
            if (!this.visited.Contains(type) && !this.toVisit.Contains(type))
            {
                this.toVisit.Enqueue(type);
            }
        }

        private string Comment(Type type)
        {
            return string.Format("{1}.{0}", type.Name, type.Namespace);
        }
        
        private MemberEventArgs GetMemberEventArgs(MemberInfo member, Type memberReturnType, Type visitingType)
        {
            var typeInfo = this.language.GetTypeInfo(memberReturnType);
            var isOwnProperty = member.DeclaringType == visitingType;
            var args = new MemberEventArgs()
            {
                MemberName = member.Name,
                MemberTypeName = typeInfo.Name,
                MemberTypeFullName = typeInfo.FullName,
                MemberTypeNameSpaceName = typeInfo.NameSpaceName,
                IgnoredByGenerators = this.IgnoreLanguages(member),
                IsOwnProperty = isOwnProperty
            };

            return args;
        }

        private TypeEventArgs GetTypeEventArgs(Type type)
        {
            var args = new TypeEventArgs()
            {
                Type = type,
                TypeName = TypeName(type),
                NameSpaceName = NameSpace(type)
            };

            if (type.BaseType != null && this.ShouldVisit(type.BaseType))
            {
                args.BaseTypeInfo = GetTypeEventArgs(type.BaseType);
            }

            return args;
        }

        /// <summary>
        /// Gets the languages that should ignore this member
        /// </summary>
        /// <param name="member"></param>
        /// <returns>
        /// List of language ids that should ignore this member when
        /// generating output
        /// </returns>
        private List<string> IgnoreLanguages(MemberInfo member)
        {
            return member.GetCustomAttributes<IgnoreForLanguageGeneratorAttribute>()
                         .Select(a => a.LanguageId)
                         .ToList();
        }

        private string NameSpace(Type type)
        {
            return this.language.GetTypeInfo(type).NameSpaceName;
        }

        private void RegisterType(Type type)
        {
            if (type.IsNullableType())
            {
                var baseType = type.GenericTypeArguments[0];
                RegisterType(baseType);
                return;
            }

            Type collectionOf;
            if (type.TryGetGenericCollectionType(out collectionOf))
            {
                RegisterType(collectionOf);
                return;
            }

            if (!ShouldVisit(type))
            {
                return;
            }

            AddIfMissing(type);

            foreach (var i in type.GetInterfaces())
            {
                RegisterType(i);
            }

            if (type.BaseType != null)
            {
                RegisterType(type.BaseType);
            }
        }

        private bool ShouldVisit(Type type)
        {
            return TypeExtensions.IsExportableType(type);
        }

        private string TypeName(Type type)
        {
            var name = this.language.GetTypeInfo(type);
            return name.Name;
        }

        private void VisitField(FieldInfo member, Type visitingType)
        {
            var args = GetMemberEventArgs(member, member.FieldType, visitingType);

            if (MemberVisiting != null) { MemberVisiting(this, args); }

            this.RegisterType(member.FieldType);

            if (MemberVisited != null) { MemberVisited(this, args); }
        }

        private void VisitMethod(MethodInfo method, Type visitingType)
        {
            var args = new MethodEventArgs()
            {
                MethodName = method.Name
            };

            if (MethodVisiting != null) { MethodVisiting(this, args); }
            // pretty sure I'll have to do something here for the controller methods input and output types
            if (MethodVisited != null) { MethodVisited(this, args); }
        }

        private void VisitProperty(PropertyInfo member, Type visitingType)
        {
            var args = GetMemberEventArgs(member, member.PropertyType, visitingType);

            if (MemberVisiting != null) { MemberVisiting(this, args); }

            this.RegisterType(member.PropertyType);

            if (MemberVisited != null) { MemberVisited(this, args); }
        }

        private void VisitType(Type type)
        {
            this.visited.Add(type);

            var nsArgs = new NameSpaceEventArgs
            {
                NameSpaceName = NameSpace(type),
                Comment = Comment(type)
            };

            var typeArgs = GetTypeEventArgs(type);

            if (NameSpaceVisiting != null) { NameSpaceVisiting(this, nsArgs); }
            if (TypeVisiting != null) { TypeVisiting(this, typeArgs); }

            foreach (var property in type.GetProperties())
            {
                VisitProperty(property, type);
            }

            foreach (var field in type.GetFields())
            {
                VisitField(field, type);
            }

            foreach (var member in type.GetMethods())
            {
                VisitMethod(member, type);
            }

            if (TypeVisited != null) { TypeVisited(this, typeArgs); }
            if (NameSpaceVisited != null) { NameSpaceVisited(this, nsArgs); }
        }
    }
}