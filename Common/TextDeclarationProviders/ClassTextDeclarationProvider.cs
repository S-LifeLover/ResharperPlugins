using System.Collections.Generic;
using System.Linq;
using Common.Providers;

namespace Common.TextDeclarationProviders
{
    internal sealed class ClassTextDeclarationProvider : IProvider<string>
    {
        public ClassTextDeclarationProvider(IEnumerable<IProvider<string>> classMemberTextDeclarationsProvider)
        {
            _classMemberTextDeclarationsProvider = classMemberTextDeclarationsProvider;
        }

        public string Get()
        {
            var classMemberTextDeclarations = GetClassMemberTextDeclarations();
            return string.Format(ClassTextDeclarationTemplate, classMemberTextDeclarations);
        }

        private string GetClassMemberTextDeclarations()
        {
            return string.Join(
                ClassMemberTextDeclarationSeparator, _classMemberTextDeclarationsProvider.Select(o => o.Get()));
        }

        private readonly IEnumerable<IProvider<string>> _classMemberTextDeclarationsProvider;

        private const string ClassTextDeclarationTemplate = @"
            private sealed class Mocks
            {{
                {0}
            }}";

        private const string ClassMemberTextDeclarationSeparator = @"

                ";
    }
}