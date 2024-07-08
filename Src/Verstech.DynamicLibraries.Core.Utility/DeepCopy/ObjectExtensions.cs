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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Verstech.DynamicLibraries.Core.Compilable;

namespace Verstech.DynamicLibraries.Core.Utility.DeepCopy
{
    public static class ObjectExtensions
    {
        static DynamicAssemblyManager _manager = new DynamicAssemblyManager("ObjectExtensions");      

        public static TResult As<TSource, TResult>(this TSource o) where TResult : new()
        {
            TResult result = new TResult();
            CopyTo(o, result);
            return result;
        }

        public static TArg CopyTo<TSrc, TArg>(this TSrc o, TArg target)
        {
            GetMapper<TSrc, TArg>()(o, target);
            return target;
        }

        private static Action<TSrc, Targ> GetMapper<TSrc, Targ>()
        {
            Type target = typeof(Targ);
            Type src = typeof(TSrc);
            var name = string.Format("{0}To{1}",Utility.GetValidString(src.FullName), Utility.GetValidString(target.FullName));

            return _manager.CallOrAdd<Action<TSrc, Targ>,Action<TSrc, Targ>>(name,name, "Copy", (a)=> a, () => 
            {
                var srcProps = src.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var trgtProps = target.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                var sr = Expression.Parameter(src, "source");
                var trgt = Expression.Parameter(target, "target");


                ClassConstructor cc = new ClassConstructor(name);
                MethodConstructor mc = new MethodConstructor("Copy",
                                        Expression.Lambda<Action<TSrc, Targ>>(
                                                Expression.Block(
                                                    (from item in trgtProps
                                                     from s in srcProps
                                                     where item.Name == s.Name && item.PropertyType == s.PropertyType
                                                     select Expression.Assign(Expression.Property(trgt, item.Name), Expression.Property(sr, item.Name)))), sr, trgt),
                                        System.Reflection.MethodAttributes.Public | System.Reflection.MethodAttributes.Static);
                cc.AddMethod(mc);

                DynamicAssembly dynamicAssembly = new DynamicAssembly(name);
                dynamicAssembly.AddReferencedType(typeof(TSrc));
                dynamicAssembly.AddReferencedType(typeof(Targ));
                dynamicAssembly.AddType(cc);

                return dynamicAssembly;
            });
        }

    }
}
