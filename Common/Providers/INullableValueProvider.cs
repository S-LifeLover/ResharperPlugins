namespace Common.Providers
{
    // ToDo_AP: ����������� �� MFR.Common
    // ToDo_AP: �������� Unchecked
    internal interface INullableValueProvider<out T>
    {
        T GetValueOrDefault();
    }
}
