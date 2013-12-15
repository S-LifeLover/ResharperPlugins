using Common.Providers;
using Common.Validators;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AutoNMock.Validators
{
    // ToDo_AP: добавить проверки на нулл (нужно узнать, как подключать решарперы)
    internal sealed class CreateMocksClassContextActionAvailabilityValidator : IValidator
    {
        // ToDo_AP: убрать зависимости selectedClassProvider и selectedVariableProvider
        public CreateMocksClassContextActionAvailabilityValidator(
            INullableValueProvider<IClassDeclaration> selectedClassProvider,
            INullableValueProvider<IVariableDeclaration> selectedVariableProvider,
            IValidator<IClassDeclaration> isClassHasTestClassAttribute,
            IValidator<IClassDeclaration> isClassContainsMocksClass,
            IValidator<IVariableDeclaration> isVariableNotNull)
        {
            _selectedClassProvider = selectedClassProvider;
            _selectedVariableProvider = selectedVariableProvider;
            _isClassHasTestClassAttribute = isClassHasTestClassAttribute;
            _isClassContainsMocksClass = isClassContainsMocksClass;
            _isVariableNotNull = isVariableNotNull;
        }

        public bool Validate()
        {
            var variableDeclaration = _selectedVariableProvider.GetValueOrDefault();
            if (!_isVariableNotNull.Validate(variableDeclaration))
                return false;

            var classDeclaration = _selectedClassProvider.GetValueOrDefault();
            if (!_isClassHasTestClassAttribute.Validate(classDeclaration))
                return false;

            return !_isClassContainsMocksClass.Validate(classDeclaration);
        }

        private readonly INullableValueProvider<IClassDeclaration> _selectedClassProvider;
        private readonly INullableValueProvider<IVariableDeclaration> _selectedVariableProvider;
        private readonly IValidator<IClassDeclaration> _isClassHasTestClassAttribute;
        private readonly IValidator<IClassDeclaration> _isClassContainsMocksClass;
        private readonly IValidator<IVariableDeclaration> _isVariableNotNull;
    }
}