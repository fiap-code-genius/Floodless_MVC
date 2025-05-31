using Floodless_MVC.Application.Interfaces;
using Floodless_MVC.Application.Voluntario;
using Floodless_MVC.Domain.Entities;
using Floodless_MVC.Domain.Interfaces;

namespace Floodless_MVC.Application.Services
{
    public class VoluntarioApplication : IVoluntarioApplication
    {
        private readonly IVoluntarioRepository _repository;

        public VoluntarioApplication(IVoluntarioRepository repository)
        {
            _repository = repository;
        }

        public VoluntarioEntity? DeletarDados(int id)
        {
            return _repository.DeletarDados(id);
        }

        public VoluntarioEntity? EditarDados(int id, VoluntarioDto voluntario)
        {
            var volunta = new VoluntarioEntity
            {
                Id = id,
                Nome = voluntario.Nome,
                Email = voluntario.Email,
                Contato = voluntario.Contato
            };

            return _repository.EditarDados(volunta);
        }

        public VoluntarioEntity? ObterPorId(int id)
        {
            return _repository.ObterPorId(id);
        }

        public IEnumerable<VoluntarioEntity> ObterTodos()
        {
            return _repository.ObterTodos() ?? Enumerable.Empty<VoluntarioEntity>();
        }

        public VoluntarioEntity? SalvarDados(VoluntarioDto voluntario)
        {
            var volunta = new VoluntarioEntity
            {
                Nome = voluntario.Nome,
                Email = voluntario.Email,
                Contato = voluntario.Contato
            };

            return _repository.SalvarDados(volunta);
        }
    }
}
