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

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vertechcon.DynamicLibraries.Utilities.DeepCopy;

namespace Vertechcon.DynamicLibraries.Compilable.Tests
{
    [TestClass]
    public class DeepCopyTests
    {

        [TestMethod]
        public void TestTypes()
        {
            Assert.IsTrue(typeof(object).IsClass);
        }


        [TestMethod]
        public void TestCopy()
        {
            var s = new Source { A = "blah", B = "Bleh", C = "34", D = "ypoyoyoyoyoyo" };
            var t = new Target();
            var res = s.As<Source, Target>();
            Assert.IsNotNull(res);
            Assert.AreEqual(s.A, res.A);
            Assert.AreEqual(s.B, res.B);
            Assert.AreEqual(0, res.C);
            Assert.IsNull(res.E);
        }

        public class Source
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
        }

        public class Target
        {
            public string A { get; set; }
            public string B { get; set; }
            public int C { get; set; }
            public string E { get; set; }
        }
    }
}
