namespace Lexicon.EntityModel
{
    public record WordRecord
    {
        public string Word { get; set; }
        
        public WordMetadata Metadata { get; set; }
    }
}
