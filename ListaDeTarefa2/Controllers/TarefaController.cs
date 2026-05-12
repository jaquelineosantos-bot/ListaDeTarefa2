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
            var tarefaDoBanco = _context.Tarefas.Find(id);

            if (tarefaDoBanco == null)
                return NotFound("Tarefa não encontrado");

            tarefaDoBanco.Descricao = tarefa.Descricao;
            tarefaDoBanco.Status = tarefa.Status;

            return Ok("Atualizado com sucesso!!");
        }
        [HttpGet("status/{nome}")]
        public IActionResult ConsultaTarefaStatus(string nome)
        {
            var tarefaDoBanco = _context.Tarefas.Where(t => t.Status.Contains(nome)).ToList();
            if (!tarefaDoBanco.Any())
                return NotFound("Tarefa não encontrado");
            return Ok(tarefaDoBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefa = _context.Usuarios.Find(id);

            if (tarefa == null)
                return NotFound("Tarefa não encontrado");

            _context.Usuarios.Remove(tarefa);
            _context.SaveChanges();

            return Ok("Deletado com sucesso ");
        }

    }
}
