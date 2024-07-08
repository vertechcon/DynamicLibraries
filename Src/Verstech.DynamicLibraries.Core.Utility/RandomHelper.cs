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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verstech.DynamicLibraries.Core.Utility
{
    public class RandomHelper
    {
        static char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray();
        static Random r = new Random();
        public static IEnumerable<char> RandomChars
        {
            get
            {
                while(true)
                    yield return chars[r.Next(0,chars.Count()-1)];
            }
        }

        public static string RandomString(int length)
        {
            StringBuilder db = new StringBuilder();
            db.Append(RandomChars.Take(length).ToArray());
            return db.ToString();
        }
    }
}
