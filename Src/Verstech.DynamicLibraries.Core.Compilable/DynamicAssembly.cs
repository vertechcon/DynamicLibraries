using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Verstech.DynamicLibraries.Core.ExpressionToSyntax;

namespace Verstech.DynamicLibraries.Core.Compilable
{
    public class DynamicAssembly : IDisposable
    {
        private readonly CollectibleAssemblyLoadContext collectibleAssemblyLoadContext = new CollectibleAssemblyLoadContext();

        private readonly List<ClassConstructor> _types = new List<ClassConstructor>();

        private readonly List<Type> _referencedTypes = new List<Type>();
        private readonly List<string> _usingDirectives = new List<string>();

        private Assembly? _builtAssembly;

        public string AssemblyName { get; private set; }

        public bool IsBuilt { get; set; }

        /// <summary>
        /// The collection of classes to be built inside the dynamic assembly
        /// </summary>
        public IEnumerable<ClassConstructor> Types { get { return _types.ToArray(); } }

        public DynamicAssembly(string name)
        {
            AssemblyName = name;
        }

        public void AddType(ClassConstructor type)
        {
            _types.Add(type);
        }

        public void AddReferencedType(Type type)
        {
            _referencedTypes.Add(type);
        }

        public void AddUsingDirective(string directive)
        {
            _usingDirectives.Add(directive);
        }

        public void Build()
        {
            if (IsBuilt)
                throw new InvalidOperationException("Cannot build an assembly that has already been built");


            var references = TypeHelper.GetReferences(_referencedTypes );
            var lst = TypeHelper.GetUsingStatementEnumerable(_usingDirectives).Select(x => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(x)));
            
            var cu = SyntaxFactory.CompilationUnit()
                .WithUsings(SyntaxFactory.List(lst))
                .NormalizeWhitespace();

            foreach (var type in _types)
            {
                cu = type.Build(cu);
            }

            var tree = SyntaxFactory.SyntaxTree(cu);


            var compilation = CSharpCompilation.Create(AssemblyName,
                                                       new[] { tree },
                                                       references,
                                                       new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .WithAssemblyName(AssemblyName);

            using var assemblyLoadContext = new CollectibleAssemblyLoadContext();
            using var ms = new MemoryStream();

            var cr = compilation.Emit(ms);

            if (!cr.Success)
            {
                throw new InvalidOperationException("Error in expression: " + cr.Diagnostics.First(e =>
                    e.Severity == DiagnosticSeverity.Error).GetMessage());
            }

            ms.Seek(0, SeekOrigin.Begin);
            _builtAssembly = assemblyLoadContext.LoadFromStream(ms);

            IsBuilt = true;
        }

        /// <summary>
        /// Generic wrapper used to unbox the created delegate into the target delegate type
        /// </summary>
        public TResult WithDelegate<TDelegate, TResult>(string className, string methodName, Func<TDelegate, TResult> func)
        {
            if(!IsBuilt)
                throw new InvalidOperationException("Cannot create a delegate from an unbuilt assembly");
            if (_builtAssembly == null)
                throw new InvalidOperationException("Built assembly cannot be null when isbuilt is true");

            var parent = _builtAssembly.GetType(className);
            var instance = _builtAssembly.CreateInstance(className);

            var propInfo = parent.GetProperty(methodName);

            var del = (TDelegate)propInfo.GetValue(instance);

            try
            {
                return func(del);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error in delegate execution: " + ex.Message);
            }
            finally
            {
                if (instance is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        /// <summary>
        /// Generic wrapper used to unbox the created delegate into the target delegate type
        /// </summary>
        public void WithDelegate<TDelegate>(string className, string methodName, Action<TDelegate> func)
        {
            if (!IsBuilt)
                throw new InvalidOperationException("Cannot create a delegate from an unbuilt assembly");
            if (_builtAssembly == null)
                throw new InvalidOperationException("Built assembly cannot be null when isbuilt is true");

            var parent = _builtAssembly.GetType(className);
            var instance = _builtAssembly.CreateInstance(className);

            var propInfo = parent.GetProperty(methodName);

            var del = (TDelegate)propInfo.GetValue(instance);

            try
            {
                func(del);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error in delegate execution: " + ex.Message);
            }
            finally
            {
                if (instance is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        public void Dispose()
        {
            if (_builtAssembly != null)
            {
                collectibleAssemblyLoadContext.Dispose();
                _builtAssembly = null;
            }
        }
    }
}
