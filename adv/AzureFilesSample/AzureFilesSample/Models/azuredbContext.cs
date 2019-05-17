using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AzureFilesSample.Models
{
    public partial class azuredbContext : DbContext
    {
        public azuredbContext()
        {
        }

        public azuredbContext(DbContextOptions<azuredbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressBook> AddressBook { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = System.Environment.GetEnvironmentVariable("SQLDB_CONNECTION");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
