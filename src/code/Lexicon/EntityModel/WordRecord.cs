namespace Lexicon.EntityModel
{
    using FluentValidation;

    public record WordRecord
    {
        public string Word { get; set; }
        
        public WordMetadata Metadata { get; set; }
    }

    public class WordRecordValidator : AbstractValidator<WordRecord>
    {
        public WordRecordValidator()
        {
            RuleFor(wr => wr.Word).NotNull().NotEmpty();
            RuleFor(wr => wr.Metadata).NotNull();
        }
    }
}
