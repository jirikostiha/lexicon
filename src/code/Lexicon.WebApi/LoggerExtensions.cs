using Microsoft.Extensions.Logging;
using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Lexicon.WebApi
{
    public static class LoggerExtensions
    {
        private static readonly Action<ILogger, int, Exception?> _gotRecordsCount;
        private static readonly Action<ILogger, int, Exception?> _exportedSize;

        static LoggerExtensions()
        {
            _gotRecordsCount = LoggerMessage.Define<int>(
                logLevel: LogLevel.Information,
                eventId: 1,
                formatString: "Got {Count} records.");

            _exportedSize = LoggerMessage.Define<int>(
               logLevel: LogLevel.Information,
               eventId: 2,
               formatString: "Exported size is {Size} bytes.");
        }

        public static void GotRecordsCount(this ILogger logger, int count)
            => _gotRecordsCount(logger, count, null);

        public static void ExportedSize(this ILogger logger, int count)
            => _exportedSize(logger, count, null);
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member