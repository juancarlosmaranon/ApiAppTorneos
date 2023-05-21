using ApiAppTorneos.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuggetAppTorneos.Models;

namespace ApiAppTorneos.Repositories
{
    public class RepositoryUsuarios
    {
        private BSTournamentContext context;

        public RepositoryUsuarios(BSTournamentContext context)
        {
            this.context = context;
        }

        public async Task<User> LoginUsuariosAsync(string email, string contrasenia)
        {
            string sql = "LOGIN_SP @Email, @Contrasenia";
            SqlParameter pamemail =
                new SqlParameter("@Email", email);
            SqlParameter pamicontrasenia =
                new SqlParameter("@Contrasenia", contrasenia);
            var consulta = await this.context.Usuarios.FromSqlRaw(sql, pamemail, pamicontrasenia).ToListAsync();
            User usuario = consulta.FirstOrDefault();
            return usuario;
        }

        public async Task InsertarUsuarioAsync(string usuariotag, string nombre, string email, string contrasenia)
        {
            string sql = "INSERTARUSUARIO_SP @UsuarioTag, @Contrasenia, @Nombre,  @Email";
            SqlParameter paminombre =
                new SqlParameter("@Nombre", nombre);
            SqlParameter pamemail =
                new SqlParameter("@Email", email);
            SqlParameter pamusuariotag =
                new SqlParameter("@UsuarioTag", usuariotag);
            SqlParameter pamicontrasenia =
                new SqlParameter("@Contrasenia", contrasenia);
            //PARA EJECUTAR CONSULTAS DE ACCION EN UN PROCEDURE
            //SE UTILIZA EL METODO ExecuteSqlRaw() Y VIENE DESDE
            // Database
            await this.context.Database.ExecuteSqlRawAsync(sql, paminombre, pamusuariotag, pamemail, pamicontrasenia);
        }

        public async Task<int> FindUsuarioXTagAsync(string tag)
        {
            User usuario = await this.context.Usuarios.Where(x => x.UsuarioTag == tag).FirstOrDefaultAsync();
            return usuario.IdUsuario;
        }


    }
}
