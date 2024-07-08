using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers
{
    public class BinaryResolvable : StringResolvable, IParentResolvable
    {
        private IStringResolvable? _right = null;
        private IStringResolvable? _left = null;
        private IStringResolvable? _conversion = null;

        public bool IsDeclaration => false;
        override public bool IsReady
        {
            get
            {
                return _right != null && _left != null && _right.IsReady && _left.IsReady && (_conversion == null || _conversion.IsReady);
            }
        }

        private ExpressionType _expressionType = ExpressionType.Assign;

        //TODO: Convert to parent and manage the type of binary expression;
        public void Configure(BinaryExpression binaryExpression, TranslatorExpressionVisitor visitor)
        {
            _expressionType = binaryExpression.NodeType;

            visitor.Visit(binaryExpression.Right);
            visitor.Visit(binaryExpression.Left);
            if (binaryExpression.Conversion != null)
            {
                visitor.Visit(binaryExpression.Conversion);
            }

            this.Verify();
        }

        override public string Resolve()
        {
            switch (this._expressionType)
            {
                case ExpressionType.Assign:
                    return $"{_left?.Resolve()} = {_right?.Resolve()}";
                case ExpressionType.Add:
                    return $"{_left?.Resolve()} + {_right?.Resolve()}";
                case ExpressionType.AddChecked:
                    return $"checked({_left?.Resolve()} + {_right?.Resolve()})";
                case ExpressionType.And:
                    return $"{_left?.Resolve()} & {_right?.Resolve()}";
                case ExpressionType.AndAlso:
                    return $"{_left?.Resolve()} && {_right?.Resolve()}";
                case ExpressionType.ArrayIndex:
                    return $"{_left?.Resolve()}[{_right?.Resolve()}]";
                case ExpressionType.Coalesce:
                    return $"({_left?.Resolve()} ?? {_right?.Resolve()})";
                case ExpressionType.Divide:
                    return $"{_left?.Resolve()} / {_right?.Resolve()}";
                case ExpressionType.Equal:
                    return $"{_left?.Resolve()} == {_right?.Resolve()}";
                case ExpressionType.ExclusiveOr:
                    return $"{_left?.Resolve()} ^ {_right?.Resolve()}";
                case ExpressionType.GreaterThan:
                    return $"{_left?.Resolve()} > {_right?.Resolve()}";
                case ExpressionType.GreaterThanOrEqual:
                    return $"{_left?.Resolve()} >= {_right?.Resolve()}";
                case ExpressionType.LeftShift:
                    return $"{_left?.Resolve()} << {_right?.Resolve()}";
                case ExpressionType.LessThan:
                    return $"{_left?.Resolve()} < {_right?.Resolve()}";
                case ExpressionType.LessThanOrEqual:
                    return $"{_left?.Resolve()} <= {_right?.Resolve()}";
                case ExpressionType.Modulo:
                    return $"{_left?.Resolve()} % {_right?.Resolve()}";
                case ExpressionType.Multiply:
                    return $"{_left?.Resolve()} * {_right?.Resolve()}";
                case ExpressionType.MultiplyChecked:
                    return $"checked({_left?.Resolve()} * {_right?.Resolve()})";
                case ExpressionType.NotEqual:
                    return $"{_left?.Resolve()} != {_right?.Resolve()}";
                case ExpressionType.Or:
                    return $"{_left?.Resolve()} | {_right?.Resolve()}";
                case ExpressionType.OrElse:
                    return $"{_left?.Resolve()} || {_right?.Resolve()}";
                case ExpressionType.Power:
                    return $"{_left?.Resolve()} ^ {_right?.Resolve()}";
                case ExpressionType.RightShift:
                    return $"{_left?.Resolve()} >> {_right?.Resolve()}";
                case ExpressionType.Subtract:
                    return $"{_left?.Resolve()} - {_right?.Resolve()}";
                case ExpressionType.SubtractChecked:
                    return $"checked({_left?.Resolve()} - {_right?.Resolve()})";
                case ExpressionType.AddAssign:
                    return $"{_left?.Resolve()} += {_right?.Resolve()}";
                case ExpressionType.AddAssignChecked:
                    return $"checked({_left?.Resolve()} += {_right?.Resolve()})";
                case ExpressionType.AndAssign:
                    return $"{_left?.Resolve()} &= {_right?.Resolve()}";
                case ExpressionType.DivideAssign:
                    return $"{_left?.Resolve()} /= {_right?.Resolve()}";
                case ExpressionType.ExclusiveOrAssign:
                    return $"{_left?.Resolve()} ^= {_right?.Resolve()}";
                case ExpressionType.LeftShiftAssign:
                    return $"{_left?.Resolve()} <<= {_right?.Resolve()}";
                case ExpressionType.ModuloAssign:
                    return $"{_left?.Resolve()} %= {_right?.Resolve()}";
                case ExpressionType.MultiplyAssign:
                    return $"{_left?.Resolve()} *= {_right?.Resolve()}";
                case ExpressionType.MultiplyAssignChecked:
                    return $"checked({_left?.Resolve()} *= {_right?.Resolve()})";
                case ExpressionType.OrAssign:
                    return $"{_left?.Resolve()} |= {_right?.Resolve()}";
                case ExpressionType.PowerAssign:
                    return $"{_left?.Resolve()} ^= {_right?.Resolve()}";
                case ExpressionType.RightShiftAssign:
                    return $"{_left?.Resolve()} >>= {_right?.Resolve()}";
                case ExpressionType.SubtractAssign:
                    return $"{_left?.Resolve()} -= {_right?.Resolve()}";
                case ExpressionType.SubtractAssignChecked:
                    return $"checked({_left?.Resolve()} -= {_right?.Resolve()})";
                default:
                    throw new InvalidOperationException("Invalid binary expression type");
            }
        }

        public override void Verify()
        {
            if (IsReady)
            {
                Ready();
            }
        }

        public void Add(IStringResolvable resolvable)
        {
            if (_right == null)
            {
                _right = resolvable;
            }
            else if (_left == null)
            {
                _left = resolvable;
            }
            else if (_conversion == null)
            {
                _conversion = resolvable;
            }
            else
            {
                throw new InvalidOperationException("Binary expression can only have two children");
            }

            resolvable.Subscribe(this.Verify);
        }
    }
}
