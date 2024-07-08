using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Verstech.DynamicLibraries.Core.ExpressionToSyntax;

namespace ExpressionTreeTests
{
    [TestClass]
    public class VisitorTests
    {
        [TestMethod]
        public void TestTranslation()
        {
            Expression<Func<int, int, int>> expr = (a, b) => a + b;

            TranslatorExpressionVisitor visitor = new TranslatorExpressionVisitor();
            visitor.Visit(expr);
            var str = visitor.Resolve();

            Assert.AreEqual("(System.Int32 a, System.Int32 b) => a + b", str);
        }

        [TestMethod]
        public void TestTranslationBlock()
        {
            var aP = Expression.Parameter(typeof(int), "a");
            var bP = Expression.Parameter(typeof(int), "b");
            var cF = Expression.Variable(typeof(int), "x");

            Expression<Func<int, int, int>> expr = Expression.Lambda<Func<int, int, int>>(
                Expression.Block(new[] { cF },
                     Expression.Assign(cF, Expression.Constant(5)),
                     Expression.Return(Expression.Label(), Expression.Add(Expression.Multiply(aP, cF), bP), typeof(int))
                ), 
                Expression.Parameter(typeof(int), "a"), 
                Expression.Parameter(typeof(int), "b"));

            TranslatorExpressionVisitor visitor = new TranslatorExpressionVisitor();
            visitor.Visit(expr);
            var str = visitor.Resolve();

            Assert.IsNotNull(str);
        }
    }
}
