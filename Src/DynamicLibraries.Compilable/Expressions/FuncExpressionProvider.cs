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

namespace Vertechcon.DynamicLibraries.Compilable.Expressions
{
    /// <summary>
    /// Expression provider implementation allowing to create an expression from a provided Function, allows creation of an expression tree from a lambda or a delegate
    /// </summary>
    public class FuncExpressionProvider<TResult> : ExpressionProvider
    {
        Func<TResult> action;

        public FuncExpressionProvider(Func<TResult> action)
        {
            this.action = action;
        }

        public override Expression CreateExpression(params Expression[] arguments)
        {
            Expression<Func<TResult>> bind = () => action();
            return Expression.Invoke(bind);
        }
    }

    /// <summary>
    /// Expression provider implementation allowing to create an expression from a provided Function, allows creation of an expression tree from a lambda or a delegate
    /// </summary>
    public class FuncExpressionProvider<TArg1, TResult> : ExpressionProvider
    {
        Func<TArg1, TResult> action;

        public FuncExpressionProvider(Func<TArg1, TResult> action)
        {
            this.action = action;
        }

        public override Expression CreateExpression(params Expression[] arguments)
        {
            Expression<Func<TArg1, TResult>> bind = (a) => action(a);
            return Expression.Invoke(bind, arguments);
        }
    }

    /// <summary>
    /// Expression provider implementation allowing to create an expression from a provided Function, allows creation of an expression tree from a lambda or a delegate
    /// </summary>
    public class FuncExpressionProvider<TArg1, TArg2, TResult> : ExpressionProvider
    {
        Func<TArg1, TArg2, TResult> action;

        public FuncExpressionProvider(Func<TArg1, TArg2, TResult> action)
        {
            this.action = action;
        }

        public override Expression CreateExpression(params Expression[] arguments)
        {
            Expression<Func<TArg1, TArg2, TResult>> bind = (a, b) => action(a, b);
            return Expression.Invoke(bind, arguments);
        }
    }

    /// <summary>
    /// Expression provider implementation allowing to create an expression from a provided Function, allows creation of an expression tree from a lambda or a delegate
    /// </summary>
    public class FuncExpressionProvider<TArg1, TArg2, TArg3, TResult> : ExpressionProvider
    {
        Func<TArg1, TArg2, TArg3, TResult> action;

        public FuncExpressionProvider(Func<TArg1, TArg2, TArg3, TResult> action)
        {
            this.action = action;
        }

        public override Expression CreateExpression(params Expression[] arguments)
        {
            Expression<Func<TArg1, TArg2, TArg3, TResult>> bind = (a, b, c) => action(a, b, c);
            return Expression.Invoke(bind, arguments);
        }
    }

    /// <summary>
    /// Expression provider implementation allowing to create an expression from a provided Function, allows creation of an expression tree from a lambda or a delegate
    /// </summary>
    public class FuncExpressionProvider<TArg1, TArg2, TArg3, TArg4, TResult> : ExpressionProvider
    {
        Func<TArg1, TArg2, TArg3, TArg4, TResult> action;

        public FuncExpressionProvider(Func<TArg1, TArg2, TArg3, TArg4, TResult> action)
        {
            this.action = action;
        }

        public override Expression CreateExpression(params Expression[] arguments)
        {
            Expression<Func<TArg1, TArg2, TArg3, TArg4, TResult>> bind = (a, b, c, d) => action(a, b, c, d);
            return Expression.Invoke(bind, arguments);
        }
    }
}
