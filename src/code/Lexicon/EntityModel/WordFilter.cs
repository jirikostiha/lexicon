using System;
using System.Collections.Generic;
using System.Linq;

namespace Lexicon.EntityModel
{
    public record MultiSourceWordFilter
    {
        public List<string>? SourceIds { get; set; }

        public WordFilter? WordFilter { get; set; } = WordFilter.Empty;

        public static MultiSourceWordFilter Empty => new()
        {
            SourceIds = null,
            WordFilter = WordFilter.Empty
        };
    }

    public record WordFilter
    {
        public Language? Language { get; set; }
        
        public WordClass? Class { get; set; }

        public string? StartsWith { get; set; }

        public static WordFilter Empty => new() 
        { 
            Language = default,
            Class = default,
            StartsWith = default
        };

        public static IEnumerable<WordRecord> Filter(IEnumerable<WordRecord> records, WordFilter filter)
        {
            return records
                .Where(x => filter.Language == null || filter.Language == x.Metadata.Language)
                .Where(x => filter.Class == null || filter.Class == x.Metadata.Class)
                .Where(x => string.IsNullOrEmpty(filter.StartsWith) || x.Word.StartsWith(filter.StartsWith, StringComparison.InvariantCulture));
        }
    }
}
