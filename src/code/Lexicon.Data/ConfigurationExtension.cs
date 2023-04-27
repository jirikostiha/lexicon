namespace Lexicon.Data
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtension
    {
        public static IEnumerable<string> GetSectionKeys(this IConfiguration configuration)
        {
            return configuration.AsEnumerable()
                .GroupBy(x => x.Key.Contains(':') ? x.Key[..x.Key.IndexOf(':')] : x.Key)
                .Select(x => x.Key);
        }
    }
}