using ApiAppTorneos.Data;
using Microsoft.EntityFrameworkCore;
using NuggetAppTorneos.Models;

namespace ApiAppTorneos.Repositories
{
    public class RepositoryLigas
    {
        private BSTournamentContext context;

        public RepositoryLigas(BSTournamentContext context)
        {
            this.context = context;
        }

        public async Task <List<Liga>> GetLigasAsync()
        {
            return await this.context.Ligas.ToListAsync();
        }

        public async Task <Liga> GetLigaAsync(int idliga)
        {
            return await this.context.Ligas.Where(x => x.IdLiga == idliga).FirstOrDefaultAsync();
        }

        public async Task <List<Equipo>> GetInfoEquiposLigaAsync(int idliga)
        {
            var consulta = (from datos in this.context.EquiposLiga
                            where datos.IdLiga == idliga
                            select datos.IdEquipo).Distinct();

            List<int> idequipos = consulta.ToList();

            return await this.context.Equipos.Where(x => idequipos.Contains(x.IdEquipo)).ToListAsync();
        }

        public async Task <List<Liga>> FiltrarLigaNombreAsync(string nombre)
        {
            return await this.context.Ligas.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
        }

        public async Task SolicitarAccesoAsync(int idliga, int idequipo)
        {
            int id = 0;
            if (this.context.EquiposLiga.Count() == 0)
            {
                id = 1;
            }
            else
            {
                id = this.context.EquiposLiga.Max(x => x.Id) + 1;
            }

            EquipoLiga solicitante = new EquipoLiga();
            solicitante.Id = id;
            solicitante.IdLiga = idliga;
            solicitante.IdEquipo = idequipo;
            solicitante.Ganados = 0;
            solicitante.Perdidos = 0;
            solicitante.Empates = 0;
            solicitante.Inscrito = false;
            this.context.EquiposLiga.Add(solicitante);
            await this.context.SaveChangesAsync();
        }

        public async Task AccionAccesoAsync(bool confirmado, int idinscrito)
        {
            EquipoLiga equipo = this.context.EquiposLiga.Where(x => x.Id == idinscrito).AsEnumerable().FirstOrDefault();
            
            if (confirmado == true)
            {
                equipo.Inscrito = true;
            }
            else
            {
                this.context.EquiposLiga.Remove(equipo);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task <List<EquipoLiga>> GetEquiposXLigaAsync(int idliga)
        {
            return await this.context.EquiposLiga.Where(x => x.IdLiga == idliga).ToListAsync();
        }

        public async Task CrearLigaAsync(string nombre, int idusuario, int idequipo)
        {
            int id = 0;
            if(this.context.Ligas.Count() == 0)
            {
                id = 1;
            }
            else
            {
                id = this.context.Ligas.Max(x => x.IdLiga) + 1;
            }

            Liga liga = new Liga();
            liga.IdLiga = id;
            liga.Nombre = nombre;
            liga.FechaInicio = null;
            liga.FechaFin = null;
            liga.Estado = -1;
            liga.Creador = idusuario;
            this.context.Ligas.Add(liga);

            await this.context.SaveChangesAsync();
            await this.AniadirEquipoDuenoLigaAsync(id, idequipo);
        }

        public async Task AniadirEquipoDuenoLigaAsync(int idliga, int idequipo)
        {
            int id = 0;
            if (this.context.EquiposLiga.Count() == 0)
            {
                id = 1;
            }
            else
            {
                id = this.context.EquiposLiga.Max(x => x.Id) + 1;
            }

            EquipoLiga solicitante = new EquipoLiga();
            solicitante.Id = id;
            solicitante.IdLiga = idliga;
            solicitante.IdEquipo = idequipo;
            solicitante.Ganados = 0;
            solicitante.Perdidos = 0;
            solicitante.Empates = 0;
            solicitante.Inscrito = true;
            this.context.EquiposLiga.Add(solicitante);

            await this.context.SaveChangesAsync();
        }

        public async Task <List<EquipoLiga>> GetEquipoLigasApuntadasAsync(int idequipo)
        {
            var consulta =  from datos in this.context.EquiposLiga
                            where datos.IdEquipo == idequipo
                            select datos;

            return consulta.ToList();
        }

        public async Task EmpezarLigaAsync(int idliga, DateTime fechainicio)
        {
            Liga liga = this.context.Ligas.Where(x => x.IdLiga == idliga).AsEnumerable().FirstOrDefault();
            liga.FechaInicio = fechainicio;
            liga.Estado = 0;

            //METODO PARA GENERAR LAS PARTIDAS DE TODOS LOS EQUIPOS
            await this.context.SaveChangesAsync();
        }


    }
}
