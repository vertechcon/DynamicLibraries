using System.Linq.Expressions;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers
{
    public class ReturnResolvable : StringResolvable, IParentResolvable
    {
        private bool _isVoid = false;
        private IStringResolvable? _returnValue = null;

        public bool IsDeclaration => false; 

        override public bool IsReady
        {
            get
            {
                return _isVoid || _returnValue != null && _returnValue.IsReady;
            }
        }

        override public string Resolve()
        {
            if (_isVoid)
            {
                return "return;";
            }
            return $"return {_returnValue?.Resolve()}";
        }

        public override void Verify()
        {
            if (IsReady)
            {
                Ready();
            }
        }
        public void Configure(GotoExpression node, TranslatorExpressionVisitor translatorExpressionVisitor)
        {
            _isVoid = node.Value == null;
            translatorExpressionVisitor.Visit(node.Value);
        }

        public void Add(IStringResolvable resolvable)
        {
            if (_isVoid || _returnValue != null)
                throw new InvalidOperationException("Return statement already has a value");
            _returnValue = resolvable;
            resolvable.Subscribe(Verify);
        }
    }
}