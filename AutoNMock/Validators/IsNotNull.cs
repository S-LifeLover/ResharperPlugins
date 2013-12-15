using Common.Validators;

namespace AutoNMock.Validators
{
    internal sealed class IsNotNull<T> : IValidator<T> where T : class
    {
        public bool Validate(T classDeclaration)
        {
            return classDeclaration != null;
        }
    }
}