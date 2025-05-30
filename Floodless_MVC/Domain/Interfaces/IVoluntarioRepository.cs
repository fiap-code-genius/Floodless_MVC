using Floodless_MVC.Domain.Entities;

namespace Floodless_MVC.Domain.Interfaces
{
    public interface IVoluntarioRepository
    {
        VoluntarioEntity? SalvarDados(VoluntarioEntity voluntario);

        IEnumerable<VoluntarioEntity> ObterTodos();

        VoluntarioEntity? ObterPorId(int id);

        VoluntarioEntity? EditarDados(VoluntarioEntity voluntario);

        VoluntarioEntity? DeletarDados(int id);
    }
}
