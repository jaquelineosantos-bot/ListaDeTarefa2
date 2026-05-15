using ListaDeTarefa2.Data;
using ListaDeTarefa2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Atividade0705.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly UsuarioContext _context;

        public UsuarioController(UsuarioContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login(Usuario dadosLogin)
        {
            var loginPadrao = _context.Usuarios.Where(u => u.Email.Equals(dadosLogin.Email) && u.Senha.Equals(dadosLogin.Senha)).ToList();

            if (loginPadrao.Count == 0)
                return Unauthorized("Email ou Senha Incorretas");
            HttpContext.Session.SetString("Email", dadosLogin.Email);
            Response.Cookies.Append("IdUsado", loginPadrao[0].Id.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None
                });
            return Ok("Login realizado com sucesso!");
        }

        [HttpGet("inicio")]
        public IActionResult Inicio()
        {
            var usuario = HttpContext.Session.GetString("Email");
            if (usuario == null)
                return Unauthorized("Não autenticado");
            return Ok("Usuário autenticado");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("usuarioEmail");

            return Ok("Logout realizado com sucesso!");
        }

        [HttpPost("cadastrar")]
        public IActionResult CriarPessoas(Usuario usuario)
        {
            _context.Add(usuario);
            _context.SaveChanges();
            return Created("Teste", usuario);
        }

        [HttpPut("atualizar/{id}")]
        public IActionResult Atualizar(int id, Usuario usuario)
        {
            var usuarioDoBanco = _context.Usuarios.Find(id);

            if (usuarioDoBanco == null)
                return NotFound("Usuário não encontrado");

            usuarioDoBanco.Email = usuario.Email;
            usuarioDoBanco.Nome = usuario.Nome;
            usuarioDoBanco.Senha = usuario.Senha;

            return Ok("Atualizado com sucesso!!");
        }
        [HttpGet("nome/{nome}")]
        public IActionResult ConsultaUsuarioNome(string nome, Usuario usuario)
        {
            var usuarioDoBanco = _context.Usuarios.Where(u => u.Nome.Contains(nome)).ToList();
            if (!usuarioDoBanco.Any())
                return NotFound("Médico não encontrado");
            return Ok(usuarioDoBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
                return NotFound("Médico não encontrado");

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return Ok("Deletado com sucesso ");
        }
    }
}
