using ApiAppTorneos.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuggetAppTorneos.Models;

#region PROCEDURES
//CREATE OR ALTER PROCEDURE INSERTARUSUARIO_SP(@UsuarioTag NVARCHAR(50), @Contrasenia NVARCHAR(50), @Nombre NVARCHAR(50), @Email NVARCHAR(50))
//AS
// INSERT INTO USUARIO VALUES ((SELECT ISNULL(MAX(IdUsuario),0) FROM USUARIO)+1, @UsuarioTag, @Contrasenia, @Nombre, @Email)
//GO

//CREATE OR ALTER PROCEDURE BUSCARUSUARIOTAG_SP(@UsuarioTag NVARCHAR(50))
//AS
//    SELECT * FROM Usuario WHERE UsuarioTag = @UsuarioTag
//GO

//CREATE OR ALTER PROCEDURE COMPROBAREMAIL_SP(@Email NVARCHAR(50))
//AS
//    SELECT * FROM Usuario WHERE Email = @Email
//GO

//CREATE OR ALTER PROCEDURE LOGIN_SP(@Email NVARCHAR(50), @Contrasenia NVARCHAR(50))
//AS
//    SELECT * FROM Usuario WHERE Email = @Email AND Contrasenia = @Contrasenia
//GO

//CREATE OR ALTER PROCEDURE DETALLESUSUARIO_SP(@IdUsuario INT)
//AS
//    SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario
//GO

//CREATE OR ALTER PROCEDURE INSERTAREQUIPO_SP(@Nombre nvarchar(50), @Jugador1 int, @Jugador2 int, @Jugador3 int)
//AS
// INSERT INTO EQUIPO VALUES((SELECT ISNULL(MAX(IdEquipo),0) FROM Equipo)+1, @Nombre, @Jugador1, @Jugador2, @Jugador3, -1, -1, 0, 0, 0)
//GO

//CREATE OR ALTER PROCEDURE SELECTEQUIPOID_SP(@IDEQUIPO INT)
//AS
//    SELECT * FROM Equipo WHERE IdEquipo = @IDEQUIPO
//GO

//CREATE OR ALTER PROCEDURE EQUIPOSUSER_SP(@IDJUGADOR INT)
//AS
//    SELECT * FROM Equipo WHERE Jugador1 = @IDJUGADOR OR Jugador2 = @IDJUGADOR OR Jugador3 = @IDJUGADOR
//GO

//CREATE OR ALTER PROCEDURE DELETEEQUIPO_SP(@IDEQUIPO INT)
//AS
// DELETE FROM Equipo WHERE IdEquipo = @IDEQUIPO
//GO
#endregion

namespace ApiAppTorneos.Repositories
{
    public class RepositoryEquipos
    {
        private BSTournamentContext context;

        public RepositoryEquipos(BSTournamentContext context)
        {
            this.context = context;
        }

        public async Task<List<Equipo>> SelectAllEquiposAsync(int idusuario) 
        {
            string sql = "EQUIPOSUSER_SP @IDJUGADOR";
            SqlParameter pamidusuario =
                new SqlParameter("@IDJUGADOR", idusuario);
            var consulta = this.context.Equipos.FromSqlRaw(sql, pamidusuario);
            List<Equipo> equipos = await consulta.ToListAsync();
            return equipos;
        }

        public async Task<Equipo> SelectEquipoAsync(int idequipo)
        {
            string sql = "SELECTEQUIPOID_SP @IDEQUIPO";
            SqlParameter pamnombre =
                new SqlParameter("@IDEQUIPO", idequipo);
            var consulta = await this.context.Equipos.FromSqlRaw(sql, pamnombre).ToListAsync();
            Equipo equip = consulta.FirstOrDefault();
            return equip;
        }

        public async Task InsertarEquipoAsync(string nombre, int jugador1, int jugador2, int jugador3)
        {
            string sql = "INSERTAREQUIPO_SP @Nombre, @Jugador1, @Jugador2, @Jugador3";
            SqlParameter paminombre =
                new SqlParameter("@Nombre", nombre);
            SqlParameter pamjug1 = 
                new SqlParameter("@Jugador1", jugador1);
            SqlParameter pamjug2 =
                new SqlParameter("@Jugador2", jugador2);
            SqlParameter pamjug3 =
                new SqlParameter("@Jugador3", jugador3);

            await this.context.Database.ExecuteSqlRawAsync(sql, paminombre, pamjug1, pamjug2, pamjug3);
        }

        public async Task DeleteEquiposAsync(int idequipo)
        {
            string sql = "DELETEEQUIPO_SP @IDEQUIPO";
            SqlParameter pamidequipo =
                new SqlParameter("@IDEQUIPO", idequipo);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamidequipo);
        }

        public async Task ConfirmaInvitacionAsync(int idequipo, bool confirmacion2, bool confirmacion3)
        {
            Equipo eq = await this.SelectEquipoAsync(idequipo);

            if (eq != null && (eq.ConfirmJug2 == -1 || eq.ConfirmJug3 == -1))
            {
                if (confirmacion2)
                {
                    eq.ConfirmJug2 = 1;
                }else if (confirmacion3)
                {
                    eq.ConfirmJug3 = 1;
                }
                else
                {
                    await this.DeleteEquiposAsync(idequipo);
                }
            }

            await this.context.SaveChangesAsync();
        }
    }
}
