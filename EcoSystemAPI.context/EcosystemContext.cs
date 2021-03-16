using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoSystemAPI.Context.Models;
using Microsoft.EntityFrameworkCore;


namespace EcoSystemAPI.Data.Context
{
    public class EcosystemContext : DbContext
    {
        public EcosystemContext(DbContextOptions<EcosystemContext> opt) : base(opt)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Merchandise> Merchandise { get; set; }
        public DbSet<Teams> Teams { get; set; }
    }
}
