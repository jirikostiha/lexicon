using System;
using System.Collections.Generic;

namespace Lexicon.EntityModel
{
    public record MultiSourceWordFilter
    {
        public List<string>? SourceIds { get; set; }

        public WordFilter WordFilter { get; set; }

        public static MultiSourceWordFilter Empty => new()
        {
            SourceIds = null,
            WordFilter = WordFilter.Empty
        };
    }

    public record WordFilter
    {
        public Language? Language { get; set; }
        
        //podstatne, pridavne, atd 
        public WordClass? Class { get; set; }

        public string? StartWith { get; set; } = string.Empty;

        public static WordFilter Empty => new() 
        { 
            Language = default,
            Class = default,
            StartWith = default
        };
    }
}
