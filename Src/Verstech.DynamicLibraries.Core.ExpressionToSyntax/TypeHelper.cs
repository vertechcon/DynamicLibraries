using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax
{
    public static class TypeHelper
    {

        private static readonly Assembly SystemRuntime = Assembly.Load(new AssemblyName("System.Runtime"));
        private static readonly Assembly NetStandard = Assembly.Load(new AssemblyName("netstandard"));

        public static string GetCSharpRepresentation(Type t, bool trimArgCount, bool fullNamespace)
        {
            if(t == typeof(void))
            {
                return "void";
            }

            if (!t.IsGenericType) return (fullNamespace ? t.FullName ?? t.Name : t.Name);
            var genericArgs = t.GetGenericArguments().ToList();

            return GetCSharpRepresentation(t, trimArgCount, genericArgs, fullNamespace);
        }

        public static string GetCSharpRepresentation(Type t, bool trimArgCount, List<Type> availableArguments, bool fullNamespace)
        {
            if (!t.IsGenericType) return (fullNamespace? t.FullName ?? t.Name : t.Name);
            var value = (fullNamespace ? t.FullName ?? t.Name : t.Name);
            if (trimArgCount && value.IndexOf("`", StringComparison.Ordinal) > -1)
            {
                value = value.Substring(0, value.IndexOf("`", StringComparison.Ordinal));
            }

            if (t.DeclaringType != null)
            {
                // This is a nested type, build the nesting type first
                value = GetCSharpRepresentation(t.DeclaringType, trimArgCount, availableArguments, fullNamespace) + "+" + value;
            }

            // Build the type arguments (if any)
            var argString = "";
            var thisTypeArgs = t.GetGenericArguments();
            for (var i = 0; i < thisTypeArgs.Length && availableArguments.Count > 0; i++)
            {
                if (i != 0) argString += ", ";

                argString += GetCSharpRepresentation(availableArguments[0], trimArgCount, fullNamespace);
                availableArguments.RemoveAt(0);
            }

            // If there are type arguments, add them with < >
            if (argString.Length > 0)
            {
                value += "<" + argString + ">";
            }

            return value;
        }

        public static string GetUsingStatements(IEnumerable<string> nonStandardUsings)
        {
            var requiredImports = new[]{ "System",
            "System.Linq",
            "System.Linq.Expressions"}.Concat(nonStandardUsings);

            var result = new StringBuilder();

            foreach (var import in requiredImports)
            {
                result.AppendLine($"using {import};");
            }

            return result.ToString();
        }

        public static IEnumerable<string> GetUsingStatementEnumerable(IEnumerable<string> nonStandardUsings)
        {
            var requiredImports = new[]{ "System",
            "System.Linq",
            "System.Linq.Expressions"}.Concat(nonStandardUsings);

            return requiredImports;
        }

        public static PortableExecutableReference[] GetReferences(IEnumerable<Type> referencedTypes)
        {
            var standardReferenceHints = new[] { typeof(string), typeof(IQueryable), typeof(IReadOnlyCollection<>), typeof(Enumerable) };
            var allHints = standardReferenceHints.Concat(referencedTypes);
            var includedAssemblies = new[] { SystemRuntime, NetStandard }.Concat(allHints.Select(t => t.Assembly)).Distinct();

            return includedAssemblies.Select(a => MetadataReference.CreateFromFile(a.Location)).ToArray();
        }
    }
}
