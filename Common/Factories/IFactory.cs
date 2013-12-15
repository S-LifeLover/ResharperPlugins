namespace Common.Factories
{
    internal interface IFactory<out TValue>
    {
        TValue Create();
    }
}
