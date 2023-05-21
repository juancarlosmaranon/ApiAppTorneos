using Microsoft.EntityFrameworkCore;
using NuggetAppTorneos.Models;

namespace ApiAppTorneos.Data
{
    public class BSTournamentContext: DbContext
    {
        public BSTournamentContext(DbContextOptions<BSTournamentContext> options) 
        : base(options){ }

        public DbSet<User> Usuarios { get; set; }
        
        public DbSet<Equipo> Equipos { get; set; }

        public DbSet<Liga> Ligas { get; set; }

        public DbSet<EquipoLiga> EquiposLiga { get; set; }
    }
}
