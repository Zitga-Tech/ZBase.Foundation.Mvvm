using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    public partial class RelayCommandDeclaration
    {
        public const string IOBSERVABLE_OBJECT_NAME = "IObservableObject";
        public const string IOBSERVABLE_OBJECT_INTERFACE = "global::ZBase.Foundation.Mvvm.IObservableObject";
        public const string RELAY_COMMAND_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.RelayCommandAttribute";

        public ClassDeclarationSyntax Syntax { get; }

        public string FullyQualifiedName { get; private set; }

        public bool IsValid { get; }

        public List<IMethodSymbol> Members { get; }

        public RelayCommandDeclaration(ClassDeclarationSyntax candidate, SemanticModel semanticModel, CancellationToken token)
        {
            var typeSymbol = semanticModel.GetDeclaredSymbol(candidate, token);

            Syntax = candidate;
            FullyQualifiedName = typeSymbol.ToFullName();
            Members = new List<IMethodSymbol>();

            var implementInterface = false;

            if (candidate.BaseList != null)
            {
                foreach (var baseType in candidate.BaseList.Types)
                {
                    var typeInfo = semanticModel.GetTypeInfo(baseType.Type, token);

                    if (typeInfo.Type.ToFullName().StartsWith(IOBSERVABLE_OBJECT_INTERFACE))
                    {
                        implementInterface = true;
                        break;
                    }
                }
            }

            if (implementInterface == false)
            {
                return;
            }

            var members = typeSymbol.GetMembers();

            foreach (var member in members)
            {
                if (member is IMethodSymbol method)
                {
                    if (method.HasAttribute(RELAY_COMMAND_ATTRIBUTE))
                    {
                        Members.Add(method);
                    }
                }
            }

            IsValid = Members.Count > 0;
        }
    }
}
