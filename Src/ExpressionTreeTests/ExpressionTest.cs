/*
    Copyright (C) 2016 Veronneau Techno. Conseil inc.
    For any questions you have regarding the solftware, feel free to get in touch by email.
    info@vertechcon.net

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Verstech.DynamicLibraries.Core.Compilable.Expressions;


namespace ExpressionTreeTests
{
    [TestClass]
    public class ExpressionTest
    {
        /// <summary>
        /// TODO: Makes thos tests clearer
        /// </summary>
        [TestMethod]
        public void TestLambda()
        {
            bool passed = false;
            Action<string,int> lmb = (string x, int y) =>
            {
                x = "flagada jhones";
                y += 1985;
                passed = true;
            };

            //this works
            lmb.Method.Invoke(Activator.CreateInstance(lmb.Method.DeclaringType), new object[] { "bleh", 45 });

            ActionExpressionProvider aep = new ActionExpressionProvider(() =>
                {
                    lmb("", 45);
                });
            
            var le = Expression.Lambda<Action>(aep.CreateExpression());
            var a = le.Compile();
            a();
            Assert.IsTrue(passed);
            passed = false;

            Func<string,string> func = (x)=>"Expected " + x;
            FuncExpressionProvider<string, string> provider = new FuncExpressionProvider<string, string>(func);
            var p = Expression.Parameter(typeof(string));
            var exp = provider.CreateExpression(p);
            var c = Expression.Lambda<Func<string,string>>(exp, p).Compile();
            Assert.AreEqual("Expected Twit", c("Twit"));
        }
        
    }
}
