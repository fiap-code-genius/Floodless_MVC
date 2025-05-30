using Floodless_MVC.Application.Dtos.Recurso;
using Floodless_MVC.Domain.Entities;

namespace Floodless_MVC.Application.Interfaces
{
    public interface IRecursoApplication
    {
        RecursoEntity? SalvarDados(RecursoDto entity);

        IEnumerable<RecursoEntity> ObterTodos();

        RecursoEntity? ObterPorId(int id);

        RecursoEntity? EditarDados(int id, RecursoDto entity);

        RecursoEntity? DeletarDados(int id);
    }
}
