namespace Lexicon.TestReady
{
    using System.Collections.Generic;
    using System.Linq;
    using Lexicon.EntityModel;
    using AutoBogus;
    using Bogus;

    public static class WordRecordGenerator
    {
        public static IList<WordRecord> GenerateRandom(int count)
        {
            var recordFaker = new AutoFaker<WordRecord>()
            .Configure(builder => builder.WithRepeatCount(count))
            .RuleFor(fake => fake.Word, fake => fake.Random.String(4, 10));

            return recordFaker.Generate(count);
        }
    }
}
