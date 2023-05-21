using ApiAppTorneos.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuggetAppTorneos.Models;

namespace ApiAppTorneos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LigaController : ControllerBase
    {
        private RepositoryLigas repo;

        public LigaController(RepositoryLigas repo)
        {
            this.repo = repo;
        }

        //GET ALL
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Liga>>> GetLigas()
        {
            return await this.repo.GetLigasAsync();
        }

        //GET Liga
        [Authorize]
        [HttpGet]   
        [Route("[action]/{idliga}")]
        public async Task<ActionResult<Liga>> GetLiga(int idliga)
        {
            return await this.repo.GetLigaAsync(idliga);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{idli}")]
        public async Task<ActionResult<List<Equipo>>> GetInfoEqLi(int idli)
        {
            return await this.repo.GetInfoEquiposLigaAsync(idli);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{nom}")]
        public async Task<ActionResult<List<Liga>>> FiltrarLiga(string nom)
        {
            return await this.repo.FiltrarLigaNombreAsync(nom);
        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{idlig}/{idequ}")]
        public async Task<ActionResult> SolicitarAcceso(int idlig, int idequ)
        {
            await this.repo.SolicitarAccesoAsync(idlig,idequ);
            return Ok();

        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{conf}/{idins}")]
        public async Task<ActionResult> AccionAcceso(bool conf, int idins)
        {
            await this.repo.AccionAccesoAsync(conf, idins);
            return Ok();

        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{idl}")]
        public async Task<ActionResult<List<EquipoLiga>>> GetEquXLig(int idl)
        {
            return await this.repo.GetEquiposXLigaAsync(idl);
        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{nombre}/{idusuario}/{idequipo}")]
        public async Task<ActionResult> CrearLiga(string nombre, int idusuario, int idequipo)
        {
            await this.repo.CrearLigaAsync(nombre, idusuario, idequipo);
            return Ok();

        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{idlig1}/{idequ1}")]
        public async Task<ActionResult> AnadirEquipoLiga(int idlig1, int idequ1)
        {
            await this.repo.AniadirEquipoDuenoLigaAsync(idlig1, idequ1);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{ideq}")]
        public async Task<ActionResult<List<EquipoLiga>>> GetEquLiAp(int ideq)
        {
            return await this.repo.GetEquipoLigasApuntadasAsync(ideq);
        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{idlig2}/{fecha}")]
        public async Task<ActionResult> EmpezarLiga(int idlig2, DateTime fecha)
        {
            await this.repo.EmpezarLigaAsync(idlig2, fecha);
            return Ok();

        }
    }
}
