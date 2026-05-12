using ListaDeTarefa2.Models;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa2.Data
{
    public class UsuarioContext:DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
                                                                                                                                  
        public DbSet<Tarefa> Tarefas { get; set; }
    }
}
