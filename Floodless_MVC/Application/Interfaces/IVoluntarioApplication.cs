using Floodless_MVC.Application.Voluntario;
using Floodless_MVC.Domain.Entities;

namespace Floodless_MVC.Application.Interfaces
{
    public interface IVoluntarioApplication
    {
        VoluntarioEntity? SalvarDados(VoluntarioDto voluntario);

        IEnumerable<VoluntarioEntity> ObterTodos();

        VoluntarioEntity? ObterPorId(int id);

        VoluntarioEntity? EditarDados(int id, VoluntarioDto voluntario);

        VoluntarioEntity? DeletarDados(int id);
    }
}
