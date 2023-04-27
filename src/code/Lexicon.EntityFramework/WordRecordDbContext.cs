namespace Lexicon.EntityFramework;

public class WordRecordContext : DbContext
{
    public WordRecordContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<WordRecord> Words => Set<WordRecord>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<WordRecord>()
               .HasOne<WordMetadata>(wr => wr.Metadata);

        base.OnModelCreating(builder);
    }
}