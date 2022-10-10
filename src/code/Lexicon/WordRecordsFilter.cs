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
            return records;

            //return records
            //    .Where(x => filter.Language == null || filter.Language == x.Metadata.Language)
            //    .Where(x => filter.Class == null || filter.Class == x.Metadata.Class)
            //    .Where(x => x.Word.StartsWith(filter.StartWith, StringComparison.InvariantCulture));
        }
    }
}
