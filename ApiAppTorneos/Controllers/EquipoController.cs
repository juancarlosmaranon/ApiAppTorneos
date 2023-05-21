using ApiAppTorneos.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuggetAppTorneos.Models;
using System.Security.Claims;

namespace ApiAppTorneos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private RepositoryEquipos repo;

        public EquipoController(RepositoryEquipos repo)
        {
            this.repo = repo;
        }

        //GET ALL
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Equipo>>> GetEquipos()
        {
            Claim claim = HttpContext.User.Claims
            .SingleOrDefault(x => x.Type == "UserData");
            string jsonUsu =
                claim.Value;
            User usuario = JsonConvert.DeserializeObject<User>
                (jsonUsu);

            return await this.repo.SelectAllEquiposAsync(usuario.IdUsuario);
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> InsertEquipo(Equipo equipo)
        {
            await this.repo.InsertarEquipoAsync(equipo.Nombre, equipo.Jugador1, equipo.Jugador2, equipo.Jugador3);
            return Ok();
        }

        //FIND Equipo
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Equipo>> FindEquipo(int id)
        {
            return await this.repo.SelectEquipoAsync(id);
        }

        //DELETE Equipo
        [Authorize]
        [HttpDelete]
        [Route("[action]/{idequipo}")]
        public async Task<ActionResult> DeleteEquipo(int idequipo)
        {
            await this.repo.DeleteEquiposAsync(idequipo);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{idequ}/{conf2}/{conf3}")]
        public async Task<ActionResult> ConfirmarEquipo(int idequ, bool conf2, bool conf3)
        {
            await this.repo.ConfirmaInvitacionAsync(idequ,conf2, conf3);
            return Ok();
        }
    }
}
