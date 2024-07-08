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

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Reflection.Emit;
using Verstech.DynamicLibraries.Core.Compilable;

namespace Verstech.DynamicLibraries.Core.Compilable
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


        private TypeSyntax getReturnType(string? returnType)
        {
            if (string.IsNullOrEmpty(returnType))
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));
            }
            else
            {
                return SyntaxFactory.ParseTypeName(returnType);
            }
        }

        /// <summary>
        /// Interceptor method used to create the method builder for a public static method from the type builder and pass it to the injection mechanism
        /// </summary>
        /// <param name="typeBuilder">The type builder that will contain the generated method</param>
        public virtual ClassDeclarationSyntax Inject(ClassDeclarationSyntax classDeclaration)
        {
            MethodDefinition methodDefinition = new MethodDefinition();
            methodDefinition.SetName(Name);
            Inject(methodDefinition);

            var md = SyntaxFactory.MethodDeclaration(getReturnType(methodDefinition.ReturnType), Name)
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(SyntaxFactory.ParseExpression(methodDefinition.MethodBody)));

            return classDeclaration.AddMembers(md);
        }

        /// <summary>
        /// The injection method to implement based on the method 
        /// </summary>
        /// <param name="methodBuilder"></param>
        public abstract void Inject(MethodDefinition methodDefinition);


    }
}
