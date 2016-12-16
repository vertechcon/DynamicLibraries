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
using Vertechcon.DynamicLibraries.Compilable.Expressions;

namespace Vertechcon.DynamicLibraries.Compilable.MethodConstructors
{

    /// <summary>
    /// Class used to inject an expression into a method body and return the associated action
    /// </summary>
    public class ExpressionProviderActionDelegateBuilder<TParam> : MethodConstructorBase, IMethodInjector
    {
        ExpressionProvider provider;

        /// <summary>
        /// Constructor for the delegate builder
        /// </summary>
        /// <param name="pro">Expression provider containing the actual logic to compile</param>
        public ExpressionProviderActionDelegateBuilder(ExpressionProvider pro)
        {
            this.provider = pro;
        }

        /// <summary>
        /// Method that creates the function skeleton from the implementation expression
        /// </summary>
        /// <returns>The action expression ready to be injected or precompiled</returns>
        protected Expression<Action<TParam>> Create()
        {
            var p0 = Expression.Parameter(typeof(TParam));

            return Expression.Lambda<Action<TParam>>(provider.CreateExpression(p0), p0);
        }

        /// <summary>
        /// Method that precompiled and returns the expression
        /// </summary>
        /// <returns>The generated action</returns>
        public Action<TParam> Build()
        {
            var ex = Create();
            return ex.Compile();
        }

        /// <summary>
        /// Injection method used to push the expression into the method builder
        /// </summary>
        /// <param name="methodBuilder"></param>
        public override void Inject(MethodBuilder methodBuilder)
        {
            var ex = Create();
            ex.CompileToMethod(methodBuilder);
        }
    }

    /// <summary>
    /// Class used to inject an expression into a method body and return the associated action
    /// </summary>
    public class ExpressionProviderActionDelegateBuilder<TParam, TResult> : MethodConstructorBase, IMethodInjector
    {
        ExpressionProvider provider;

        /// <summary>
        /// Constructor for the delegate builder
        /// </summary>
        /// <param name="pro">Expression provider containing the actual logic to compile</param>
        public ExpressionProviderActionDelegateBuilder(ExpressionProvider pro)
        {
            this.provider = pro;
        }

        /// <summary>
        /// Method that creates the function skeleton from the implementation expression
        /// </summary>
        /// <returns>The action expression ready to be injected or precompiled</returns>
        protected Expression<Action<TParam, TResult>> Create()
        {
            var p0 = Expression.Parameter(typeof(TParam));
            var p1 = Expression.Parameter(typeof(TResult));

            return Expression.Lambda<Action<TParam, TResult>>(provider.CreateExpression(p0, p1), p0, p1);
        }

        /// <summary>
        /// Method that precompiled and returns the expression
        /// </summary>
        /// <returns>The generated action</returns>
        public Action<TParam, TResult> Build()
        {
            var ex = Create();
            return ex.Compile();
        }

        /// <summary>
        /// Injection method used to push the expression into the method builder
        /// </summary>
        /// <param name="methodBuilder"></param>
        public override void Inject(MethodBuilder methodBuilder)
        {
            var ex = Create();
            ex.CompileToMethod(methodBuilder);
        }
    }

    /// <summary>
    /// Class used to inject an expression into a method body and return the associated action
    /// </summary>
    public class ExpressionProviderActionDelegateBuilder<TParam, TParam1, TResult> : MethodConstructorBase, IMethodInjector
    {

        ExpressionProvider provider;

        /// <summary>
        /// Constructor for the delegate builder
        /// </summary>
        /// <param name="pro">Expression provider containing the actual logic to compile</param>
        public ExpressionProviderActionDelegateBuilder(ExpressionProvider pro)
        {
            this.provider = pro;
        }

        /// <summary>
        /// Method that creates the function skeleton from the implementation expression
        /// </summary>
        /// <returns>The action expression ready to be injected or precompiled</returns>
        protected Expression<Action<TParam, TParam1, TResult>> Create()
        {
            var p0 = Expression.Parameter(typeof(TParam));
            var p1 = Expression.Parameter(typeof(TParam1));
            var p2 = Expression.Parameter(typeof(TResult));

            return Expression.Lambda<Action<TParam, TParam1, TResult>>(provider.CreateExpression(p0, p1, p2), p0, p1, p2);
        }

        /// <summary>
        /// Method that precompiled and returns the expression
        /// </summary>
        /// <returns>The generated action</returns>
        public Action<TParam, TParam1, TResult> Build()
        {
            var ex = Create();
            return ex.Compile();
        }

        /// <summary>
        /// Injection method used to push the expression into the method builder
        /// </summary>
        /// <param name="methodBuilder"></param>
        public override void Inject(MethodBuilder methodBuilder)
        {
            var ex = Create();
            ex.CompileToMethod(methodBuilder);
        }
    }

    /// <summary>
    /// Class used to inject an expression into a method body and return the associated action
    /// </summary>
    public class ExpressionProviderActionDelegateBuilder<TParam, TParam1, TParam2, TResult> : MethodConstructorBase, IMethodInjector
    {
        ExpressionProvider provider;

        /// <summary>
        /// Constructor for the delegate builder
        /// </summary>
        /// <param name="pro">Expression provider containing the actual logic to compile</param>
        public ExpressionProviderActionDelegateBuilder(ExpressionProvider pro)
        {
            this.provider = pro;
        }

        /// <summary>
        /// Method that creates the function skeleton from the implementation expression
        /// </summary>
        /// <returns>The action expression ready to be injected or precompiled</returns>
        protected Expression<Action<TParam, TParam1, TParam2, TResult>> Create()
        {
            var p0 = Expression.Parameter(typeof(TParam));
            var p1 = Expression.Parameter(typeof(TParam1));
            var p2 = Expression.Parameter(typeof(TParam2));
            var p3 = Expression.Parameter(typeof(TResult));

            return Expression.Lambda<Action<TParam, TParam1, TParam2, TResult>>(provider.CreateExpression(p0, p1, p2, p3), p0, p1, p2, p3);
        }

        /// <summary>
        /// Method that precompiled and returns the expression
        /// </summary>
        /// <returns>The generated action</returns>
        public Action<TParam, TParam1, TParam2, TResult> Build()
        {
            var ex = Create();
            return ex.Compile();
        }

        /// <summary>
        /// Injection method used to push the expression into the method builder
        /// </summary>
        /// <param name="methodBuilder"></param>
        public override void Inject(MethodBuilder methodBuilder)
        {
            var ex = Create();
            ex.CompileToMethod(methodBuilder);
        }
    }

    /// <summary>
    /// Class used to inject an expression into a method body and return the associated action
    /// </summary>
    public class ExpressionProviderActionDelegateBuilder<TParam, TParam1, TParam2, TParam3, TResult> : MethodConstructorBase, IMethodInjector
    {
        ExpressionProvider provider;

        /// <summary>
        /// Constructor for the delegate builder
        /// </summary>
        /// <param name="pro">Expression provider containing the actual logic to compile</param>
        public ExpressionProviderActionDelegateBuilder(ExpressionProvider pro)
        {
            this.provider = pro;
        }

        /// <summary>
        /// Method that creates the function skeleton from the implementation expression
        /// </summary>
        /// <returns>The action expression ready to be injected or precompiled</returns>
        protected Expression<Action<TParam, TParam1, TParam2, TParam3, TResult>> Create()
        {
            var p0 = Expression.Parameter(typeof(TParam));
            var p1 = Expression.Parameter(typeof(TParam1));
            var p2 = Expression.Parameter(typeof(TParam2));
            var p3 = Expression.Parameter(typeof(TParam3));
            var p4 = Expression.Parameter(typeof(TResult));

            return Expression.Lambda<Action<TParam, TParam1, TParam2, TParam3, TResult>>(provider.CreateExpression(p0, p1, p2, p3, p4), p0, p1, p2, p3, p4);
        }

        /// <summary>
        /// Method that precompiled and returns the expression
        /// </summary>
        /// <returns>The generated action</returns>
        public Action<TParam, TParam1, TParam2, TParam3, TResult> Build()
        {
            var ex = Create();
            return ex.Compile();
        }

        /// <summary>
        /// Injection method used to push the expression into the method builder
        /// </summary>
        /// <param name="methodBuilder"></param>
        public override void Inject(MethodBuilder methodBuilder)
        {
            var ex = Create();
            ex.CompileToMethod(methodBuilder);
        }
    }
}
