using Floodless_MVC.Domain.Entities;

namespace Floodless_MVC.Domain.Interfaces
{
    public interface IRecursoRepository
    {
        RecursoEntity? SalvarDados(RecursoEntity entity);

        IEnumerable<RecursoEntity> ObterTodos();

        RecursoEntity? ObterPorId(int id);

        RecursoEntity? EditarDados(RecursoEntity entity);

        RecursoEntity? DeletarDados(int id);
    }
}
