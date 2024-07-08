using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verstech.DynamicLibraries.Core.ExpressionToSyntax;

namespace Verstech.DynamicLibraries.Core.Compilable
{
    public class MethodDefinition
    {
        public string Name { get; private set; }
        public string ReturnType { get; private set; }

        public MethodAttributes MethodAttributes { get; private set; }

        public Tuple<string, string>[] Parameters { get; private set; } 

        public string MethodBody { get; private set; }

        public MethodDefinition()
        {
            
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void SetReturnType(Type returnType)
        {
            this.ReturnType = TypeHelper.GetCSharpRepresentation(returnType, true, true);
        }

        public void SetMethodBody(string methodBody)
        {
            this.MethodBody = methodBody;
        }   

        public void SetMethodAttributes(MethodAttributes attributes)
        {
            this.MethodAttributes = attributes;
        }

        public void SetParameters(IEnumerable<Tuple<string, Type>> parameters)
        {
            this.Parameters = parameters.Select(p => new Tuple<string, string>(p.Item1, TypeHelper.GetCSharpRepresentation(p.Item2, true, true))).ToArray();
        }
    }
}
