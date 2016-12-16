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
using System.Reflection.Emit;

namespace Vertechcon.DynamicLibraries.Compilable
{
    /// <summary>
    /// Abstract class to provides the necessary functionality to implement compilable expression trees and retrieve delegates from the generated libraries
    /// </summary>
    public abstract class MethodConstructorBase
    {
        /// <summary>
        /// The name of the method to construct
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The parent type containing the method
        /// </summary>
        Type _parentType;

        /// <summary>
        /// Interceptor method used to create the method builder for a public static method from the type builder and pass it to the injection mechanism
        /// </summary>
        /// <param name="typeBuilder">The type builder that will contain the generated method</param>
        public virtual void Inject(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod(Name, System.Reflection.MethodAttributes.Public | System.Reflection.MethodAttributes.Static);
            Inject(methodBuilder);
        }

        /// <summary>
        /// The injection method to implement based on the method 
        /// </summary>
        /// <param name="methodBuilder"></param>
        public abstract void Inject(MethodBuilder methodBuilder);

        /// <summary>
        /// Injects the <see cref="MethodConstructor"/> with the parent type of the method
        /// </summary>
        /// <param name="t">The type to be used as container type for the implemented method</param>
        public void SetType(Type t)
        {
            this._parentType = t;
        }

        /// <summary>
        /// Requests a delegate from the method defined in the underlying type
        /// </summary>
        /// <param name="delegateType">delegate type to conform to</param>
        /// <param name="instance">The instance to use if the method is not static</param>
        /// <returns>The requested delegate</returns>
        public Delegate CreateDelegate(Type delegateType, object instance = null)
        {
            var methodInfo = _parentType.GetMethod(this.Name);
            if (instance != null)
                return methodInfo.CreateDelegate(delegateType, instance);
            else
                return methodInfo.CreateDelegate(delegateType);
        }

        /// <summary>
        /// Generic wrapper used to unbox the created delegate into the target delegate type
        /// </summary>
        /// <typeparam name="TDelegate">The required delegate type</typeparam>
        /// <param name="instance">The instance from which to invoke the method</param>
        /// <returns>A usable delegate</returns>
        public TDelegate CreateDelegate<TDelegate>(object instance = null)
        {
            var methodInfo = _parentType.GetMethod(this.Name);
            if (instance != null)
                return (TDelegate)(object)methodInfo.CreateDelegate(typeof(TDelegate), instance);
            else
                return (TDelegate)(object)methodInfo.CreateDelegate(typeof(TDelegate));
        }
    }
}
