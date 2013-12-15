namespace Common.Providers
{
    // ToDo_AP: скопировано из MFR.Common
    // ToDo_AP: Пометить Unchecked
    internal interface INullableValueProvider<out T>
    {
        T GetValueOrDefault();
    }
}
