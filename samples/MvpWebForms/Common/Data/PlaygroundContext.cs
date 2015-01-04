// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using MvpWebForms.Entities;

    public class MvpWebFormsContext : DbContext
    {
        public MvpWebFormsContext() : base(nameOrConnectionString: "SqlServer") { }

        public DbSet<Widget> Widgets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
