using Floodless_MVC.Application.Dtos.Recurso;
using Floodless_MVC.Application.Interfaces;
using Floodless_MVC.Domain.Entities;
using Floodless_MVC.Domain.Interfaces;

namespace Floodless_MVC.Application.Services
{
    public class RecursoApplication : IRecursoApplication
    {
        private readonly IRecursoRepository _repository;

        public RecursoApplication(IRecursoRepository repository)
        {
            _repository = repository;
        }

        public RecursoEntity? DeletarDados(int id)
        {
            return _repository.DeletarDados(id);
        }

        public RecursoEntity? EditarDados(int id, RecursoDto entity)
        {
            var recurso = new RecursoEntity
            {
                Id = id,
                Nome = entity.Nome,
                TipoRecurso = entity.TipoRecurso,
                Quantidade = entity.Quantidade,
                VoluntarioId = entity.VoluntarioId
            };

            return _repository.EditarDados(recurso);
        }

        public RecursoEntity? ObterPorId(int id)
        {
            return _repository.ObterPorId(id);
        }

        public IEnumerable<RecursoEntity> ObterTodos()
        {
            return _repository.ObterTodos() ?? Enumerable.Empty<RecursoEntity>();
        }

        public RecursoEntity? SalvarDados(RecursoDto entity)
        {
            var recurso = new RecursoEntity
            {
                Nome = entity.Nome,
                TipoRecurso = entity.TipoRecurso,
                Quantidade = entity.Quantidade,
                VoluntarioId = entity.VoluntarioId
            };

            return _repository.SalvarDados(recurso);
        }
    }
}
