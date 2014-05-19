namespace Playground.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Playground.Model;

    public class PlaygroundContext : DbContext
    {
        public PlaygroundContext() : base(nameOrConnectionString: "SqlServer") { }

        public DbSet<Widget> Widgets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
