using System.Linq.Expressions;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers
{
    public class MemberAssignmentResolvable : StringResolvable
    {
        override public bool IsReady
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_value);
            }
        }

        private string _value = "";

        public void Configure(MemberAssignment expr)
        {
            _value = expr.ToString();
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