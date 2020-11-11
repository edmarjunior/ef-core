using CursoEFCore.CursoEFCore;
using CursoEFCore.Data.Configurations;
using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data
{
    public class ApplicationContext: DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=Vendas;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* an option for mapping in separed files */

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            /* another option for mapping in this method */

            // modelBuilder.Entity<Cliente>(p => {
            //     p.ToTable("Clientes");
            //     p.HasKey(p => p.Id);
            //     p.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired();
            //     p.Property(p => p.Telefone).HasColumnType("CHAR(11)");
            //     p.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
            //     p.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
            //     p.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

            //     p.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone");
            // });
        }
    }
}
