using _5417_Sagenergy.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace _5417_Sagenergy.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }

        // Representa a tabela de clientes na base de dados
        public DbSet<Client> Clients { get; set; }
    }
}
