using _5417_Sagenergy.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace _5417_Sagenergy.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }

        // Representa a tabela de clientes na base de dados
        public DbSet<Client> Clients { get; set; }

        // Representa a tabela de pedidos de assistência
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        // Representa a tabela dos serviços associados a cada pedido
        public DbSet<ServiceRequestDetail> ServiceRequestDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Client)
                .WithOne(client => client.User)
                .HasForeignKey<Client>(client => client.UserId)
                .IsRequired(false);
        }
    }
}