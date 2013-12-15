using Common.Providers;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AutoNMock.Providers
{
    internal sealed class SelectedVariableProvider : INullableValueProvider<IVariableDeclaration>
    {
        public SelectedVariableProvider(IContextActionDataProvider contextActionDataProvider)
        {
            _contextActionDataProvider = contextActionDataProvider;
        }

        public IVariableDeclaration GetValueOrDefault()
        {
            return _contextActionDataProvider.GetSelectedElement<IVariableDeclaration>(false, true);
        }

        private readonly IContextActionDataProvider _contextActionDataProvider;
    }
}