using System.Linq.Expressions;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers
{
    public class ParameterResolvable : StringResolvable
    {
        override public bool IsReady
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_value);
            }
        }

        private string _value = "";

        public void Configgure(ParameterExpression expr, bool isDeclaration)
        {
            if (isDeclaration)
                _value = $"{TypeHelper.GetCSharpRepresentation(expr.Type, true, true)} {expr.Name}";
            else
                _value = expr.Name;
            
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