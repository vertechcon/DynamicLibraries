using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verstech.DynamicLibraries.Core.ExpressionToSyntax.StringResolvers
{
    public interface IStringResolvable
    {
        bool IsReady { get; }
        string Resolve();
        void Subscribe(Action ready);

        string ResolvableType { get; }
    }

    public interface IParentResolvable: IStringResolvable
    {
        void Add(IStringResolvable resolvable);
        bool IsDeclaration { get; }
    }

    public abstract class StringResolvable : IStringResolvable
    {
        protected Action Ready { get; private set; }
        public abstract bool IsReady { get; }

        public abstract string Resolve();

        public string ResolvableType => this.GetType().Name;

        public void Subscribe(Action ready)
        {
            Ready = ready;
        }

        public abstract void Verify();
    }

}
