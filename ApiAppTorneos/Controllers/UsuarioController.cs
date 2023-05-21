    using ApiAppTorneos.Helpers;
using ApiAppTorneos.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuggetAppTorneos.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiAppTorneos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private RepositoryUsuarios repo;
        private HelperLogin helper;

        public UsuarioController(RepositoryUsuarios repo, HelperLogin helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            User usu =
                await this.repo.LoginUsuariosAsync
                (model.UserName, model.Password);
            if (usu == null)
            {
                return Unauthorized();
            }
            else
            {
                string jsonUsu = JsonConvert.SerializeObject(usu);
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonUsu)
                };
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken()
                    , SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: informacion,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }

        [HttpPost]
        [Route("[action]/{usuariotag}/{nombre}/{email}/{contrasenia}")]
        public async Task<ActionResult> InsertarUsu(string usuariotag, string nombre, string email, string contrasenia)
        {
            await this.repo.InsertarUsuarioAsync(usuariotag, nombre, email, contrasenia);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{tag}")]
        public async Task<ActionResult<int>> FindUsu(string tag)
        {
            return await this.repo.FindUsuarioXTagAsync(tag);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<User>> PerfilUsuario()
        {
            //DEBEMOS BUSCAR EL CLAIM DEL EMPLEADO
            Claim claim = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonUser =
                claim.Value;
            User usuario = JsonConvert.DeserializeObject<User>
                (jsonUser);
            return usuario;
        }
    }
}
