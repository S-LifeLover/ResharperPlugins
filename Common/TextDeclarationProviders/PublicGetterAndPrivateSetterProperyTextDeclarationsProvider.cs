using System.Collections.Generic;
using System.Linq;
using Common.Providers;
using JetBrains.ReSharper.Psi;

namespace Common.TextDeclarationProviders
{
    internal sealed class PublicGetterAndPrivateSetterProperyTextDeclarationsProvider : IProvider<string>
    {
        public PublicGetterAndPrivateSetterProperyTextDeclarationsProvider(
            IProvider<IEnumerable<ITypeOwner>> properties)
        {
            _properties = properties;
        }

        public string Get()
        {
            var properiesTextDeclaration =
                _properties.Get().Select(
                    property => string.Format(
                        PropertyTemplate,
                        property.Type.GetScalarType().GetClrName().ShortName,
                        property.ShortName));
            // ToDo_AP: Ахтунг!!! Дублирование!!!
            return string.Join(ClassMemberTextDeclarationSeparator, properiesTextDeclaration);
        }

        private readonly IProvider<IEnumerable<ITypeOwner>> _properties;

        private const string PropertyTemplate = @"public {0} {1} {{ get; private set; }}";

        // ToDo_AP: Ахтунг!!! Дублирование!!!
        private const string ClassMemberTextDeclarationSeparator = @"

";
    }
}