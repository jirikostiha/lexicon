using System;
using System.Collections.Generic;

namespace Lexicon.EntityModel
{
    public record MultiSourceWordFilter
    {
        public List<string>? SourceIds { get; set; }

        public WordFilter WordFilter { get; set; } = WordFilter.Empty;

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
    }
}
