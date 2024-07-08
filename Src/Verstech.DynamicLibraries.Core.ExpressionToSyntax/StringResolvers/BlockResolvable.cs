using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers
{

    public class BlockResolvable : StringResolvable, IParentResolvable
    {

        private readonly List<IStringResolvable> _fields = new List<IStringResolvable>();
        private readonly List<IStringResolvable> _resolvables = new List<IStringResolvable>();

        private bool _isFields = true;

        public bool IsDeclaration => _isFields;

        public void Add(IStringResolvable resolvable)
        {
            if (_isFields)
                _fields.Add(resolvable);
            else
                _resolvables.Add(resolvable);
            resolvable.Subscribe(Verify);
        }
        public override string Resolve()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (var field in _fields)
            {
                sb.AppendLine($"{field.Resolve()};");
            }

            foreach (var resolvable in _resolvables)
            {
                sb.AppendLine($"{resolvable.Resolve()};");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        public void Configgure(BlockExpression node, TranslatorExpressionVisitor visitor)
        {
            _isFields = true;
            foreach (var expr in node.Variables)
            {
                visitor.Visit(expr);
            }
            _isFields = false;
            foreach (var expr in node.Expressions)
            {
                visitor.Visit(expr);
            }
        }

        public override bool IsReady
        {
            get
            {
                return _resolvables.All(r => r.IsReady);
            }
        }

        public override void Verify()
        {
            if (IsReady && Ready != null)
                Ready();
        }

    }
}
