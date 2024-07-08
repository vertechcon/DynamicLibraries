using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers
{
    public class MemberResolvable : StringResolvable
    {
        override public bool IsReady
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_value);
            }
        }

        private string _value = "";

        public void Configgure(MemberExpression expr, bool isDeclaration)
        {
            
            StringBuilder stringBuilder = new StringBuilder();
            if (isDeclaration)
            {
                if (expr.Member is FieldInfo)
                {
                    var fi = expr.Member as FieldInfo;
                    if((fi.Attributes & FieldAttributes.Static) == FieldAttributes.Static)
                    {
                        stringBuilder.Append("static ");
                    }
                    if ((fi.Attributes & FieldAttributes.Public) == FieldAttributes.Public)
                    {
                        stringBuilder.Append("public ");
                    }
                    if ((fi.Attributes & FieldAttributes.Private) == FieldAttributes.Private)
                    {
                        stringBuilder.Append("private ");
                    }
                    if ((fi.Attributes & FieldAttributes.Family) == FieldAttributes.Family)
                    {
                        stringBuilder.Append("protected ");
                    }
                }
                else if (expr.Member is PropertyInfo)
                {
                    var pi = expr.Member as PropertyInfo;
                    if (pi.GetMethod.IsStatic)
                    {
                        stringBuilder.Append("static ");
                    }
                    if (pi.GetMethod.IsPublic)
                    {
                        stringBuilder.Append("public ");
                    }
                    if (pi.GetMethod.IsPrivate)
                    {
                        stringBuilder.Append("private ");
                    }
                    if (pi.GetMethod.IsFamily)
                    {
                        stringBuilder.Append("protected ");
                    }
                }
                stringBuilder.Append($"{TypeHelper.GetCSharpRepresentation(expr.Type, true, true)} {expr.Member.Name}");
            }
            else
            {
                bool isStatic = false;
                if (expr.Member is FieldInfo)
                {
                    var fi = expr.Member as FieldInfo;
                    isStatic = fi.IsStatic;
                }
                else if (expr.Member is PropertyInfo)
                {
                var pi = expr.Member as PropertyInfo;
                    isStatic = pi.GetMethod.IsStatic;
                }
                
                if (isStatic)
                {
                    stringBuilder.Append($"{TypeHelper.GetCSharpRepresentation(expr.Member.DeclaringType, true, true)}.{expr.Member.Name}");
                }
                else
                {

                    if(expr.Expression != null)
                    {
                        TranslatorExpressionVisitor translatorExpressionVisitor = new TranslatorExpressionVisitor();
                        translatorExpressionVisitor.Visit(expr.Expression);
                        stringBuilder.Append(translatorExpressionVisitor.Resolve());
                        stringBuilder.Append(".");
                        stringBuilder.Append(expr.Member.Name);
                    }
                    else
                    {
                        stringBuilder.Append("this.");
                        stringBuilder.Append(expr.Member.Name);
                    }
                }
            }

            _value = stringBuilder.ToString();          
            
            this.Verify();
        }

        override public string Resolve()
        {
            return _value;
        }

        public override void Verify()
        {
            if (IsReady)
            {
                Ready();
            }
        }
    }
}