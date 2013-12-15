namespace Common.Validators
{
    internal interface IValidator<in T>
    {
        // ToDo_AP: Пометить Unchecked
        bool Validate(T obj);
    }
}