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

namespace Vertechcon.DynamicLibraries.Compilable
{
    /// <summary>
    /// Used to manage a dynamic assembly from building the logic to compiling it 
    /// </summary>
    public class DynamicAssemblyManager
    {
        /// <summary>
        /// The assembly builder used to generate the assembly
        /// </summary>
        System.Reflection.Emit.AssemblyBuilder _dynAss;
        
        /// <summary>
        /// The module builder used to generate the DLL
        /// </summary>
        System.Reflection.Emit.ModuleBuilder _module;

        /// <summary>
        /// The collection of classes to be built inside the dynamic assembly
        /// </summary>
        List<ClassConstructor> _types = new List<ClassConstructor>();

        /// <summary>
        /// The dll / assembly name
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Specifies if the dll has already been built
        /// </summary>
        public bool IsBuilt { get; set; }

        /// <summary>
        /// The collection of classes to be built inside the dynamic assembly
        /// </summary>
        public IEnumerable<ClassConstructor> Types { get { return _types.ToArray(); } }

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
        public void AddType(ClassConstructor type)
        {
            _types.Add(type);
            IsBuilt = IsBuilt && type.IsBuilt;
        }

        /// <summary>
        /// Creates the dynamic assembly and associated module using the referenced app domain
        /// </summary>
        /// <param name="domain"></param>
        private void MakeModule(AppDomain domain)
        {
            if (_module == null)
            {
                _dynAss = domain.DefineDynamicAssembly(new System.Reflection.AssemblyName(AssemblyName), System.Reflection.Emit.AssemblyBuilderAccess.RunAndSave);
                _module = _dynAss.DefineDynamicModule(AssemblyName, AssemblyName + ".dll");
            }
        }

        /// <summary>
        /// Builds the assembly and all underlying classes
        /// </summary>
        /// <param name="domain"></param>
        public void Build(AppDomain domain)
        {
            MakeModule(domain);
            foreach (var type in _types)
                if(!type.IsBuilt)
                    type.Build(_module);
            IsBuilt = true;
        }

        /// <summary>
        /// Builds a single specified class
        /// </summary>
        /// <param name="domain">The app domain from which to generate the assembly and module</param>
        /// <param name="className">The class to build</param>
        public void BuildClass(AppDomain domain, string className)
        {
            MakeModule(domain);
            var cc = _types.Find(x => x.Name == className);
            cc.Build(_module);
        }
    }
}
