using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using THAMCOMVC.Models;

namespace THAMCOMVC.Data
{
    public class AccountContext : DbContext
    {
        public AccountContext (DbContextOptions<AccountContext> options)
            : base(options)
        {
        }

        public DbSet<THAMCOMVC.Models.User> User { get; set; } = default!;

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Default fallback configuration
                optionsBuilder.UseSqlite("Data Source=localdb.sqlite");
            }
        }
    }
}
