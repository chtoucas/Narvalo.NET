namespace Narvalo.Narrative
{
    using CommandLine;

    public sealed class AppOptions
    {
        [OptionArray]
        public string[] Paths { get; set; }
    }
}
