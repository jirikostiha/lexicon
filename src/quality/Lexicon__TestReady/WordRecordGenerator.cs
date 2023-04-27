namespace Lexicon.TestReady
{
    using AutoBogus;
    using Lexicon.EntityModel;
    using System.Collections.Generic;

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