﻿namespace Lexicon.Data
{
    using System;
    using System.Globalization;
    using CsvHelper.Configuration;

    public class CsvHelperWordProviderOptions
    {
        /// <summary> Base name </summary>
        public const string BaseName = "CsvHelper";

        public string File { get; init; }

        public CsvConfiguration CsvConfiguration { get; set; } = new CsvConfiguration(CultureInfo.InvariantCulture);
    }
}
