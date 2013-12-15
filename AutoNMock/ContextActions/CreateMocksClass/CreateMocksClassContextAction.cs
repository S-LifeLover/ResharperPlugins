using AutoNMock.Providers;
using AutoNMock.Validators;
using Common.ContextActions;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AutoNMock.ContextActions.CreateMocksClass
{
    [ContextAction(
        Group = "C#",
        Name = "Create mocks class",
        Description = "Adds Mocks class with sut-object and its dependencies")]
    public class CreateMocksClassContextAction : IContextAction
    {
        public CreateMocksClassContextAction(ICSharpContextActionDataProvider provider)
        {
            _createMocksClassContextActionLogic =
                new ValidatorBasedContextActionLogic(
                    new CreateMocksClassContextActionAvailabilityValidator(
                        new SelectedClassProvider(
                            provider),
                        new SelectedVariableProvider(
                            provider),
                        new IsClassHasTestClassAttribute(),
                        new IsClassContainsMocksClass(),
                        new IsNotNull<IVariableDeclaration>()),
                    new PrototypeBulbItemImpl(provider));
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            return _createMocksClassContextActionLogic.IsAvailable(cache);
        }

        public IBulbItem[] Items
        {
            get { return _createMocksClassContextActionLogic.Items; }
        }

        private readonly IContextAction _createMocksClassContextActionLogic;
    }
}
