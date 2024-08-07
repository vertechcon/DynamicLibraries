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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;
using Verstech.DynamicLibraries.Core.Compilable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Verstech.DynamicLibraries.Core.ExpressionToSyntax;

namespace Verstech.DynamicLibraries.Core.Compilable
{
    /// <summary>
    /// Constructor object used to help manage dynamic method composition when genereting a dynamic precompiled type
    /// </summary>
    public class MethodConstructor : MethodConstructorBase
    {
     
        /// <summary>
        /// The lambda expression containing the method body and definission
        /// </summary>
        LambdaExpression _expression;

        /// <summary>
        /// The attributes that define the method within the assembly
        /// </summary>
        MethodAttributes _attributes;


        /// <summary>
        /// Main assembly constructor
        /// </summary>
        /// <param name="name">The name of the method to construct</param>
        /// <param name="expression">The lambda expression containing the method body and definission</param>
        /// <param name="attributes">The attributes that define the method within the assembly</param>
        public MethodConstructor(string name, LambdaExpression expression, MethodAttributes attributes)
        {
            this.Name = name;
            this._expression = expression;
            this._attributes = attributes;
        }


        /// <summary>
        /// Method allowing the caller to compile the lambda to a method builder, defining the method in its parent type
        /// </summary>
        /// <param name="typeBuilder"></param>
        public override ClassDeclarationSyntax Inject(ClassDeclarationSyntax classDeclaration)
        {
            MethodDefinition methodDefinition = new MethodDefinition();
            methodDefinition.SetMethodAttributes(MethodAttributes.Public);
            methodDefinition.SetName(Name);
            methodDefinition.SetReturnType(_expression.ReturnType);
            methodDefinition.SetParameters(_expression.Parameters.Select(p => new Tuple<string, Type>(p.Name, p.Type)).ToArray());
            
            Inject(methodDefinition);

            var fd = SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.ParseTypeName(TypeHelper.GetCSharpRepresentation(_expression.Type, true, true)),
                        SyntaxFactory.SeparatedList(new[] { SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier($"fld{Name}"), null,
                            SyntaxFactory.EqualsValueClause(SyntaxFactory.ParseExpression(methodDefinition.MethodBody))) })))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)));

            classDeclaration = classDeclaration.AddMembers(fd);


            var md = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(TypeHelper.GetCSharpRepresentation(_expression.Type, true, true)), Name)
                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.ThisExpression(),
                        SyntaxFactory.IdentifierName($"fld{Name}"))))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));  

            //if (_attributes.HasFlag(MethodAttributes.Public))
            //    md = md.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));
            //if (_attributes.HasFlag(MethodAttributes.Static))
            //    md = md.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.StaticKeyword)));
            //if (_attributes.HasFlag(MethodAttributes.Virtual))
            //    md = md.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.VirtualKeyword)));
            //if (_attributes.HasFlag(MethodAttributes.Final))
            //    md = md.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.SealedKeyword)));
            //if (_attributes.HasFlag(MethodAttributes.Abstract))
            //    md = md.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.AbstractKeyword)));


            return classDeclaration.AddMembers(md);
        }

        /// <summary>
        /// Injects the expression into the method builder
        /// </summary>
        /// <param name="methodBuilder"></param>
        public override void Inject(MethodDefinition methodBuilder)
        {
            TranslatorExpressionVisitor translatorExpressionVisitor = new TranslatorExpressionVisitor();
            translatorExpressionVisitor.Visit(_expression);
            methodBuilder.SetMethodBody(translatorExpressionVisitor.Resolve());

        }
    }
}
