using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Common.ReSharper.Psi.CSharp
{
    internal sealed class CSharpElementFactoryWrapper : ICSharpElementFactory
    {
        public CSharpElementFactoryWrapper(IPsiModule module)
        {
            _cSharpElementFactory = CSharpElementFactory.GetInstance(module);
        }

        public ICSharpTypeMemberDeclaration CreateTypeMemberDeclaration(string format, params object[] args)
        {
            return _cSharpElementFactory.CreateTypeMemberDeclaration(format, args);
        }

        private readonly CSharpElementFactory _cSharpElementFactory;
    }
}