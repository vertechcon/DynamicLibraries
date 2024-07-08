using System.Linq.Expressions;
using System.Text;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers
{
    internal class LambdaResolvable<T> : StringResolvable, IParentResolvable
    {
        private readonly List<IStringResolvable> _parameters = new List<IStringResolvable>();
        private readonly List<IStringResolvable> _body = new List<IStringResolvable>();

        private bool _isParams = true;
        
        public bool IsDeclaration => _isParams;
        override public bool IsReady
        {
            get
            {
                return _parameters.All(r => r.IsReady) && _body.All(r => r.IsReady);
            }
        }

        

        public void Configgure(Expression<T> expr, TranslatorExpressionVisitor visitor)
        {
            _isParams = true;

            foreach (var param in  expr.Parameters)
                visitor.Visit(param);

            _isParams = false;

            visitor.Visit(expr.Body);

            this.Verify();
        }

        override public string Resolve()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            stringBuilder.Append(string.Join(", ", _parameters.Select(p => p.Resolve())));
            stringBuilder.Append(") => ");
            foreach (var resolvable in _body)
            {
                stringBuilder.Append(resolvable.Resolve());
            }
            return stringBuilder.ToString();
        }

        public override void Verify()
        {
            if (IsReady && Ready != null)
            {
                Ready();
            }
        }

        public void Add(IStringResolvable resolvable)
        {
            if (_isParams)
            {
                _parameters.Add(resolvable);
            }
            else
            {
                _body.Add(resolvable);
            }
            resolvable.Subscribe(Verify);
        }
    }
}