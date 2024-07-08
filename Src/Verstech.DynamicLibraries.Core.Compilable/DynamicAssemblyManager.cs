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
using Verstech.DynamicLibraries.Core.Compilable;

namespace Verstech.DynamicLibraries.Core.Compilable
{
    /// <summary>
    /// Used to manage a dynamic assembly from building the logic to compiling it 
    /// </summary>
    public class DynamicAssemblyManager: IDisposable
    {
        
        /// <summary>
        /// The collection of classes to be built inside the dynamic assembly
        /// </summary>
        List<DynamicAssembly> _assemblies = new List<DynamicAssembly>();

        /// <summary>
        /// The dll / assembly name
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Specifies if the dll has already been built
        /// </summary>
        public bool IsBuilt { get; set; }

        

        /// <summary>
        /// Main ctor
        /// </summary>
        /// <param name="name">Dll name to use for the dynamic library</param>
        public DynamicAssemblyManager(string name)
        {
            AssemblyName = name;
        }

        /// <summary>
        /// Add a class definition to the assembly
        /// </summary>
        /// <param name="type"></param>
        public void AddAndBuildAssembly(DynamicAssembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            if (_assemblies.Any(a => a.AssemblyName == assembly.AssemblyName))
                throw new InvalidOperationException("An assembly with the same name already exists in the collection.");

            _assemblies.Add(assembly);
            assembly.Build();
        }

        public TResult CallOrAdd<TDelegate, TResult>(string assemblyName, string className, string methodName, Func<TDelegate, TResult> func, Func<DynamicAssembly> factory)
        {
            if (!_assemblies.Any(a => a.AssemblyName == assemblyName))
            {
                var assembly = factory();

                if(assembly.AssemblyName != assemblyName)
                    throw new InvalidOperationException("The assembly name provided by the factory does not match the assembly name provided in the method call.");

                AddAndBuildAssembly(assembly);
            }

            return WithDelegate(assemblyName, className, methodName, func);
        }

        public void CallOrAdd<TDelegate>(string assemblyName, string className, string methodName, Action<TDelegate> func, Func<DynamicAssembly> factory)
        {
            if (!_assemblies.Any(a => a.AssemblyName == assemblyName))
            {
                var assembly = factory();

                if (assembly.AssemblyName != assemblyName)
                    throw new InvalidOperationException("The assembly name provided by the factory does not match the assembly name provided in the method call.");

                AddAndBuildAssembly(assembly);
            }

            WithDelegate(assemblyName, className, methodName, func);
        }


        public TResult WithDelegate<TDelegate, TResult>(string assemblyName, string className, string methodName, Func<TDelegate, TResult> func)
        {
            return _assemblies.First(a => a.AssemblyName == assemblyName).WithDelegate(className, methodName, func);
        }

        public void WithDelegate<TDelegate>(string assemblyName, string className, string methodName, Action<TDelegate> func)
        {
            _assemblies.First(a => a.AssemblyName == assemblyName).WithDelegate(className, methodName, func);
        }

        public void Dispose()
        {
            foreach (var assembly in _assemblies)
            {
                assembly.Dispose();
            }
            _assemblies.Clear();
        }
    }
}
