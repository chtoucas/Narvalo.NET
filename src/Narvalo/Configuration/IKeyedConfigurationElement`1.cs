namespace Narvalo.Configuration
{
    public interface IKeyedConfigurationElement<TKey>
    {
        TKey Key { get; }
    }
}
