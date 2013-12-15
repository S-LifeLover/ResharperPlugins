using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Common.ReSharper.Psi.CSharp
{
    internal interface ICSharpElementFactory
    {
        ICSharpTypeMemberDeclaration CreateTypeMemberDeclaration(string format, params object[] args);
    }
}
