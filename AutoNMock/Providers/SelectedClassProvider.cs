using Common.Providers;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AutoNMock.Providers
{
    internal sealed class SelectedClassProvider : INullableValueProvider<IClassDeclaration>
    {
        public SelectedClassProvider(IContextActionDataProvider contextActionDataProvider)
        {
            _contextActionDataProvider = contextActionDataProvider;
        }

        public IClassDeclaration GetValueOrDefault()
        {
            return _contextActionDataProvider.GetSelectedElement<IClassDeclaration>(false, true);
        }

        private readonly IContextActionDataProvider _contextActionDataProvider;
    }
}