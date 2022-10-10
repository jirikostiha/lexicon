namespace Lexicon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Lexicon.EntityModel;

    public static class WordRecordsFilter
    {
        public static IEnumerable<WordRecord> Filter(IEnumerable<WordRecord> records, WordFilter filter)
        {
            return records
                .Where(x => x.Metadata.Language == filter.Language)
                .Where(x => x.Metadata.Class == filter.Class)
                .Where(x => x.Word.StartsWith(filter.StartWith, StringComparison.InvariantCulture));
        }
    }
}
