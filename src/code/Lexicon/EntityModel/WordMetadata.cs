namespace Lexicon.EntityModel
{
    public record WordMetadata
    {
        public Language Language { get; set; }

        public WordClass Class { get; set; }
    }
}
