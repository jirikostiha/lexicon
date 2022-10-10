namespace Lexicon.TestReady
{
    using System.Collections.Generic;
    using System.Linq;
    using Lexicon.EntityModel;

    public static class WordSets
    {
        public static IEnumerable<WordRecord> All => 
            CzechMaleNames
            .Union(EnglishMaleNames)
            .Union(EnglishSolarSystemPlanetNames);

        public static IEnumerable<WordRecord> CzechMaleNames
        {
            get
            {
                yield return new() { Word = "Josef", Metadata = new() { Language = Language.Czech, Class = WordClass.Noun } };
                yield return new() { Word = "Jan", Metadata = new() { Language = Language.Czech, Class = WordClass.Noun } };
                yield return new() { Word = "Horymír", Metadata = new() { Language = Language.Czech, Class = WordClass.Noun } };
                yield return new() { Word = "Bohuslav", Metadata = new() { Language = Language.Czech, Class = WordClass.Noun } };
            }
        }

        public static IEnumerable<WordRecord> EnglishMaleNames
        {
            get
            {
                yield return new() { Word = "James", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "Robert", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "John", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "Michael", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "David", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "William", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
            }
        }

        public static IEnumerable<WordRecord> EnglishSolarSystemPlanetNames
        {
            get
            {
                yield return new() { Word = "Mercury", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "Venus", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "Earth", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "Mars", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "Jupiter", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "Saturn", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "Uranus", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
                yield return new() { Word = "Neptune", Metadata = new() { Language = Language.English, Class = WordClass.Noun } };
            }
        }
    }
}
