using System.Linq.Expressions;
using System.Text;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers
{
    public class ConditionalResolvable : StringResolvable, IParentResolvable
    {
        private IStringResolvable _test;
        private IStringResolvable _ifTrue;
        private IStringResolvable _ifFalse;

        public bool IsDeclaration => false;
        public void Add(IStringResolvable resolvable)
        {
            if(_test == null)
            {
                _test = resolvable;
            }
            else if(_ifTrue == null)
            {
                _ifTrue = resolvable;
            }
            else
            {
                _ifFalse = resolvable;
            }
            resolvable.Subscribe(Verify);
        }

        public void Configure(ConditionalExpression expr)
        {
            
        }

        public override string Resolve()
        {
            
            StringBuilder sb = new StringBuilder();
            sb.Append("if(");
            sb.Append(_test.Resolve());
            sb.AppendLine(")");
            sb.AppendLine("{");
            sb.AppendLine(_ifTrue.Resolve());
            sb.AppendLine("}");
            sb.AppendLine("else");
            sb.AppendLine("{");
            sb.AppendLine(_ifFalse.Resolve());
            sb.AppendLine("}");

            return sb.ToString();
        }

        public override bool IsReady
        {
            get
            {
                return _test.IsReady && _ifTrue.IsReady && _ifFalse.IsReady;
            }
        }

        public override void Verify()
        {
            if (IsReady && Ready != null)
                Ready();
        }
    }
}