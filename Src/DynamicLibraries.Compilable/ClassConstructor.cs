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
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Vertechcon.DynamicLibraries.Compilable
{
    /// <summary>
    /// Support class used to keep track of method definitions and compile them into the specified type using the provided ModuleBuilder
    /// </summary>
    public class ClassConstructor
    {
        /// <summary>
        /// The methods to be compiled
        /// </summary>
        List<MethodConstructorBase> _methods = new List<MethodConstructorBase>();

        /// <summary>
        /// The name of the type to build
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Specifies if the type has already been built
        /// </summary>
        public bool IsBuilt { get; private set; }

        /// <summary>
        /// Main ctor
        /// </summary>
        /// <param name="name">The name of the type to create</param>
        public ClassConstructor(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Exposes the methods to be built 
        /// </summary>
        public IEnumerable<MethodConstructorBase> Methods
        {
            get { return this._methods.ToArray(); }
        }

        /// <summary>
        /// Adds a method definition to the collection to build
        /// </summary>
        /// <param name="method"></param>
        public void AddMethod(MethodConstructorBase method)
        {
            if (IsBuilt)
                throw new InvalidOperationException("Cannot add a method definition to a previously built class");
            _methods.Add(method);
        }

        /// <summary>
        /// The actual built class type
        /// </summary>
        public Type ActualType { get; private set; }
        
        /// <summary>
        /// Routine that creates the type, injects the method definitions into it and compiles it to a dynamic assembly
        /// </summary>
        /// <param name="builder"></param>
        public void Build(ModuleBuilder builder)
        {
            if (IsBuilt)
                throw new InvalidOperationException("Cannot rebuild a previously built class");

            //Check if the assembly was already compiled
            if (!builder.Assembly.DefinedTypes.Any(x => x.Name == Name))
            {
                //Not compiled, define the type and inject the methods into it
                var b = builder.DefineType(Name);
                foreach (var m in _methods)
                    m.Inject(b);
                //Compile the type
                var t = b.CreateType();
                ActualType = t;
            }
            else // The assembly was already compiled, retrieve the type
                ActualType = builder.Assembly.GetType(Name);

            //Set the actual type in the method definition to allow creation of the delegates
            foreach (var m in _methods)
                m.SetType(ActualType);

            //Set the IsBuilt variable to true
            IsBuilt = true;
        }
    }
}
