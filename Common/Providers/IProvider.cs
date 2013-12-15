namespace Common.Providers
{
    internal interface IProvider<out T>
    {
        T Get();
    }
}
