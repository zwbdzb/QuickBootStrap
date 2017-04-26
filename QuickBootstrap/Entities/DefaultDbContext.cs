using System.Data.Entity;

namespace QuickBootstrap.Entities
{
    /// <summary>
    /// 默认数据上下文
    /// </summary>
    public class DefaultDbContext : DbContext
    {

        static DefaultDbContext()
        {
         //   Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DefaultDbContext>());
        }
     
        public DbSet<User> User { get; set; }

        public DbSet<SalesData>  SalesData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesData>().Property(x => x.Comm).HasPrecision(18, 4);
            modelBuilder.Entity<SalesData>().Property(x => x.Price).HasPrecision(18, 2);
            modelBuilder.Entity<SalesData>().Property(x => x.Sales).HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);
        }

    }
}