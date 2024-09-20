using Microsoft.EntityFrameworkCore;
using WsWeb.Models;

namespace WsWeb.Context
{
    /*Criação do contexto do BD, aqui é feito a configuração para que o BD crie as tabelas e seja feito a relação de uma tabela para outra e caso 
    queira deletar algo, seja feito em cascata*/
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medico>()
                .HasMany(m => m.Pacientes)
                .WithOne(m => m.Medico)
                .HasForeignKey(m => m.MedicoId)
                .OnDelete(DeleteBehavior.Cascade);
                
            base.OnModelCreating(modelBuilder); 
        }
    }
}
