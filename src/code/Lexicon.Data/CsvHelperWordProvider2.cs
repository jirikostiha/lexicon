namespace Lexicon.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CsvHelper;
    using Lexicon.EntityModel;

    //[next]
    public class CsvHelperWordProvider : IWordProvider, IDisposable
    {
        private readonly CsvHelperWordProviderOptions _options;
        private readonly FileSystemWatcher _watcher;
        private int? _count; // cached number
        private bool disposedValue;

        public CsvHelperWordProvider(CsvHelperWordProviderOptions options)
        {
            _options = options;

            var watcher = new FileSystemWatcher("", _options.File)
            {
                NotifyFilter = NotifyFilters.Size
            };
            watcher.Changed += new FileSystemEventHandler(FileChanged);
            watcher.EnableRaisingEvents = true;

            _watcher = watcher; 
        }

        public async Task<long> CountAsync(CancellationToken ct = default)
        {
            if (_count is null)
            {
                var records = await GetRecordsAsync(ct)
                    .ConfigureAwait(false);
                _count = records.Count();
            }

            return _count.Value;
        }

        public async Task<IEnumerable<WordRecord>> GetByFilterAsync(WordFilter filter, CancellationToken ct = default)
        {
            var records = await GetRecordsAsync(ct)
                .ConfigureAwait(false);
            
            return WordFilter.Filter(records, filter);
        }

        private async Task<IEnumerable<WordRecord>> GetRecordsAsync(CancellationToken ct = default)
        {
            var content = File.ReadAllText(_options.File);
            using var textReader = new StringReader(content);
            using var csvReader = new CsvReader(textReader, _options.CsvConfiguration);

            var records = await csvReader.GetRecordsAsync<WordRecord>(ct)
                .ToArrayAsync(ct)
                .ConfigureAwait(false);
            
            return records;
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name == _options.File && e.ChangeType == WatcherChangeTypes.Changed)
                _count = null; // invalidate the value
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    _watcher.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
