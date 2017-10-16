using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Munin.DAL.SQLite.Models;

namespace Munin.DAL.SQLite
{
    public class MuninLiteContext : DbContext
    {
        public DatabaseContext() :
            base(new SQLiteConnection() { ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = "D:\\Databases\\SQLiteWithEF.db", ForeignKeys = true }.ConnectionString     }, true)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Archive> Archives { get; set; }

        public DbSet<ArchiveDocument> ArchiveDocuments { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Cadaster> Cadasters { get; set; }

        public DbSet<ChirchRecord> ChirchRecords { get; set; }

        public DbSet<Clip> Clips { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<Map> Maps { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Sequence> Sequences { get; set; }
    }
}
  