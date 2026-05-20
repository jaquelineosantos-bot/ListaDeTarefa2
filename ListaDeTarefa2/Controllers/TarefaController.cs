using ListaDeTarefa2.Data;
using ListaDeTarefa2.Models;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeTarefa2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly UsuarioContext _context;

        public TarefaController(UsuarioContext context)
        {
            _context = context;
        }

        [HttpPost("criar")]
        public IActionResult CriarPessoas(Tarefa tarefa)
        {
            var usuario = HttpContext.Session.GetString("Email");
            if (usuario == null)
                return Unauthorized("Não autenticado");

            var sessao = Request.Cookies["IdUsado"];
            if (sessao != null)
            {
                tarefa.IdUsuario = int.Parse(sessao);
            }

            _context.Add(tarefa);
            _context.SaveChanges();
            return Created("Teste", tarefa);
        }

        [HttpPut("atualizar/{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var usuario = HttpContext.Session.GetString("Email");
            if (usuario == null)
                return Unauthorized("Não autenticado");

            var tarefaDoBanco = _context.Tarefas.Find(id);

            if (tarefaDoBanco == null)
                return NotFound("Tarefa não encontrado");

            tarefaDoBanco.Descricao = tarefa.Descricao;
            tarefaDoBanco.Statuss = tarefa.Statuss;
            _context.SaveChanges();
            return Ok("Atualizado com sucesso!!");
        }


        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var usuario = HttpContext.Session.GetString("Email");
            if (usuario == null)
                return Unauthorized("Não autenticado");

            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound("Tarefa não encontrado");

            _context.Tarefas.Remove(tarefa);
            _context.SaveChanges();

            return Ok("Deletado com sucesso ");
        }

        [HttpGet("reservasCliente")]
        public IActionResult ReservasCliente()
        {
            var usuario = HttpContext.Session.GetString("Email");
            if (usuario == null)
                return Unauthorized("Não autenticado");


            var idCliente = Request.Cookies["IdUsado"];
            if (idCliente != null)
            {


                var resultado = from u in _context.Usuarios
                                join t in _context.Tarefas
                                on u.Id equals t.IdUsuario
                                where  u.Id == int.Parse(idCliente)
                                select new
                                {
                                    Usuario = u.Nome,
                                    u.Email,
                                    Tarefa = t.Descricao,
                                    t.Statuss,
                                    t.Id

                                };
                return Ok(resultado.ToList());
            }

            return Unauthorized("Não autenticado");

        }
    }
}
