using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Verstech.DynamicLibraries.Core.Compilable
{
    public class CollectibleAssemblyLoadContext : AssemblyLoadContext, IDisposable
    {
        public CollectibleAssemblyLoadContext() : base(true)
        { }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }

        public void Dispose()
        {
            Unload();
        }
    }
}
