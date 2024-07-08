using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq.Expressions;
using System.Reflection;
using Verstech.DynamicLibraries.Core.Compilable;
using Verstech.DynamicLibraries.Core.ExpressionToSyntax;

namespace ExpressionTreeTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SimpleExpressionTest()
        {
            var expr = Expression.Lambda<Func<int>>(Expression.Add(Expression.Constant(1), Expression.Constant(2)));

            var str = expr.ToString();
            str.Should().Be("() => (1 + 2)");
        }


        [TestMethod]
        public void SimpleExpressionTest2()
        {
            var parameter = Expression.Parameter(typeof(int), "x");
            var expr = Expression.Lambda<Func<int, int>>(Expression.Add(parameter, Expression.Constant(2)), parameter);

            var str = expr.ToString();
            str.Should().Be("x => (x + 2)");

            Func<int,int> lbd = x => x + 2;

        }

        [TestMethod]
        public void SimpleExpressionTest3()
        {
            var parameter = Expression.Parameter(typeof(int), "x");
            var expr = Expression.Lambda<Func<int, int>>(Expression.Add(parameter, Expression.Constant(2)), parameter);
            var exprRes = SyntaxFactory.ParseExpression(expr.ToString());
            exprRes.Should().NotBeNull();

            var cl = SyntaxFactory.ClassDeclaration("TestChildClass").WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));
            cl = cl.AddMembers(SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName("Func<int, int>"), "TestMethod")
                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(exprRes))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))));
            cl.Should().NotBeNull();



            var references = TypeHelper.GetReferences(new[] { typeof(int) });
            var lst = TypeHelper.GetUsingStatementEnumerable(new string[] { }).Select(x=> SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(x)));
            var cu = SyntaxFactory.CompilationUnit().AddMembers(cl)
                .WithUsings(SyntaxFactory.List(lst))
                .NormalizeWhitespace();


            var tree = SyntaxFactory.SyntaxTree(cu);
            

            var compilation = CSharpCompilation.Create("FilterCompiler_" + Guid.NewGuid(),
                                                       new[] { tree},
                                                       references,
                                                       new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .WithAssemblyName("This.Is.My.TestAssembly");

            using var assemblyLoadContext = new CollectibleAssemblyLoadContext();
            using var ms = new MemoryStream();

            var cr = compilation.Emit(ms);

            if (!cr.Success)
            {
                throw new InvalidOperationException("Error in expression: " + cr.Diagnostics.First(e =>
                    e.Severity == DiagnosticSeverity.Error).GetMessage());
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = assemblyLoadContext.LoadFromStream(ms);

            var outerClassType = assembly.GetType("TestChildClass");

            var inst = Activator.CreateInstance(outerClassType);

            var exprField = outerClassType.GetProperty("TestMethod");



            // ReSharper disable once PossibleNullReferenceException
            var method = (Func<int,int>)exprField.GetValue(inst);
            method(4).ToString().Should().Be("6");
            method.Should().NotBeNull();
        }
    }
}