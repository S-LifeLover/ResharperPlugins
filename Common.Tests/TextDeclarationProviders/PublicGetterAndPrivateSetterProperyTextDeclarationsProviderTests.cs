using System.Collections.Generic;
using Common.Providers;
using Common.Tests.TestTools;
using Common.TextDeclarationProviders;
using JetBrains.ReSharper.Psi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Tests.TextDeclarationProviders
{
    [TestClass]
    public class PublicGetterAndPrivateSetterProperyTextDeclarationsProviderTests : TestsWithMocks
    {
        // ToDO_AP: тест ужасен. Нужно уменьшить ответственность.
        [TestMethod]
        public void CreateTest()
        {
            var properties = MockFactory.CreateMock<IProvider<IEnumerable<ITypeOwner>>>();
            var sut = new PublicGetterAndPrivateSetterProperyTextDeclarationsProvider(properties.MockObject);

            var property1 = MockFactory.CreateMock<ITypeOwner>();
            var property2 = MockFactory.CreateMock<ITypeOwner>();
            properties.Expects.One
                .MethodWith(o => o.Get())
                .WillReturn(new List<ITypeOwner>
                    {
                        property1.MockObject, 
                        property2.MockObject
                    });

            var type1 = MockFactory.CreateMock<IType>();
            property1.Expects.One.GetProperty(o => o.Type).WillReturn(type1.MockObject);
            var declaredType1 = MockFactory.CreateMock<IDeclaredType>();
            type1.Expects.One.MethodWith(o => o.GetScalarType()).WillReturn(declaredType1.MockObject);
            var clrTypeName1 = MockFactory.CreateMock<IClrTypeName>();
            declaredType1.Expects.One.MethodWith(o => o.GetClrName()).WillReturn(clrTypeName1.MockObject);
            clrTypeName1.Expects.One.GetProperty(o => o.ShortName).WillReturn("SomeTypeName1");
            property1.Expects.One.GetProperty(o => o.ShortName).WillReturn("SomePropertyName1");

            var type2 = MockFactory.CreateMock<IType>();
            property2.Expects.One.GetProperty(o => o.Type).WillReturn(type2.MockObject);
            var declaredType2 = MockFactory.CreateMock<IDeclaredType>();
            type2.Expects.One.MethodWith(o => o.GetScalarType()).WillReturn(declaredType2.MockObject);
            var clrTypeName2 = MockFactory.CreateMock<IClrTypeName>();
            declaredType2.Expects.One.MethodWith(o => o.GetClrName()).WillReturn(clrTypeName2.MockObject);
            clrTypeName2.Expects.One.GetProperty(o => o.ShortName).WillReturn("SomeTypeName2");
            property2.Expects.One.GetProperty(o => o.ShortName).WillReturn("SomePropertyName2");

            var actualResult = sut.Get();
            Assert.AreEqual(ExpectedResult, actualResult);
        }

        private const string ExpectedResult = @"public SomeTypeName1 SomePropertyName1 { get; private set; }

public SomeTypeName2 SomePropertyName2 { get; private set; }";
    }
}
