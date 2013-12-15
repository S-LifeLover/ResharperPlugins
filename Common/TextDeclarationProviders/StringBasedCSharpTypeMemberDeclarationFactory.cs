using Common.Factories;
using Common.Providers;
using Common.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Common.TextDeclarationProviders
{
    internal class StringBasedCSharpTypeMemberDeclarationFactory : IFactory<ICSharpTypeMemberDeclaration>
    {
        public StringBasedCSharpTypeMemberDeclarationFactory(
            ICSharpElementFactory cSharpElementFactory, IProvider<string> typeMemberTextDeclarationProvider)
        {
            _cSharpElementFactory = cSharpElementFactory;
            _typeMemberTextDeclarationProvider = typeMemberTextDeclarationProvider;
        }

        public ICSharpTypeMemberDeclaration Create()
        {
            var textDeclaration = _typeMemberTextDeclarationProvider.Get();
            return _cSharpElementFactory.CreateTypeMemberDeclaration(textDeclaration);
        }

        private readonly ICSharpElementFactory _cSharpElementFactory;
        private readonly IProvider<string> _typeMemberTextDeclarationProvider;
    }
}
