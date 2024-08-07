﻿/*
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Verstech.DynamicLibraries.Core.Compilable.Expressions
{
    /// <summary>
    /// Base class used to provide a standardized frame for expression construction
    /// </summary>
    public abstract class ExpressionProvider
    {
        protected static Type ActivatorType = typeof(Activator);

        public abstract System.Linq.Expressions.Expression CreateExpression(params System.Linq.Expressions.Expression[] arguments);
    }
}
