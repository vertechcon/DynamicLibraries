using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax
{
    public class TranslatorExpressionVisitor : ExpressionVisitor
    {
        private IStringResolvable root;

        private Stack<IParentResolvable> parents = new Stack<IParentResolvable>();

        public bool IsReady
        {
            get;
            private set;
        }

        public TranslatorExpressionVisitor()
        {
        }

        public string Resolve()
        {
            return root.Resolve();
        }

        protected void PreProcess(IStringResolvable resolvable)
        {
            if (root == null)
            {
                root = resolvable;
                root.Subscribe(() => { this.IsReady = true; });
            }
            else if (parents.Count > 0)
            {
                parents.Peek().Add(resolvable);
            }
            else
            {
                throw new InvalidOperationException("Parent stack is empty");
            }
        }


        protected override Expression VisitBlock(BlockExpression node)
        {
            var block = new BlockResolvable();

            PreProcess(block);
            parents.Push(block);

            block.Configgure(node, this);

            parents.Pop();
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            BinaryResolvable binary = new BinaryResolvable();
            PreProcess(binary);
            parents.Push(binary);
            binary.Configure(node, this);
            parents.Pop();
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            ConstantResolvable constant = new ConstantResolvable();
            PreProcess(constant);

            constant.Configgure(node);

            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterResolvable parameter = new ParameterResolvable();
            PreProcess(parameter);

            parameter.Configgure(node, parents != null && parents.Count > 0 && parents.Peek().IsDeclaration);

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            MethodCallResolvable methodCall = new MethodCallResolvable();
            PreProcess(methodCall);

            methodCall.Configgure(node);

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            MemberResolvable member = new MemberResolvable();
            PreProcess(member);

            member.Configgure(node, parents != null && parents.Count > 0 && parents.Peek().IsDeclaration);

            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            UnaryResolvable unary = new UnaryResolvable();
            PreProcess(unary);

            unary.Configgure(node);

            return node;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            LambdaResolvable<T> lambda = new LambdaResolvable<T>();
            PreProcess(lambda);
            parents.Push(lambda);
            lambda.Configgure(node, this);

            return node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            NewResolvable newResolvable = new NewResolvable();
            PreProcess(newResolvable);

            newResolvable.Configgure(node);

            return node;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            MemberInitResolvable memberInit = new MemberInitResolvable();
            PreProcess(memberInit);

            memberInit.Configure(node);

            return node;
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            ListInitResolvable listInit = new ListInitResolvable();
            PreProcess(listInit);

            listInit.Configgure(node);

            return node;
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            ConditionalResolvable conditional = new ConditionalResolvable();
            PreProcess(conditional);

            parents.Push(conditional);

            Visit(node.Test);
            Visit(node.IfTrue);
            Visit(node.IfFalse);

            parents.Pop();
            return node;
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            MemberAssignmentResolvable memberInit = new MemberAssignmentResolvable();
            PreProcess(memberInit);

            memberInit.Configure(node);

            return node;
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            switch (node.Kind)
            {
                case GotoExpressionKind.Return:
                    ReturnResolvable returnResolvable = new ReturnResolvable();
                    PreProcess(returnResolvable);
                    parents.Push(returnResolvable);
                    returnResolvable.Configure(node, this);
                    parents.Pop();
                    break;
            }
            return node;
        }

    }
}
