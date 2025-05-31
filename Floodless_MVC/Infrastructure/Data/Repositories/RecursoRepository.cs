using Floodless_MVC.Domain.Entities;
using Floodless_MVC.Domain.Interfaces;
using Floodless_MVC.Infrastructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace Floodless_MVC.Infrastructure.Data.Repositories
{
    public class RecursoRepository : IRecursoRepository
    {
        private readonly ApplicationContext _context;

        public RecursoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public RecursoEntity? DeletarDados(int id)
        {
            try
            {
                var recurso = _context.Recurso.Find(id);

                if (recurso is not null)
                {
                    _context.Recurso.Remove(recurso);
                    _context.SaveChanges();

                    return recurso;
                }

                throw new Exception("Não foi possivel localizar o recurso para seguir com a exclusão");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public RecursoEntity? EditarDados(RecursoEntity entity)
        {
            try
            {
                var recurso = _context.Recurso.Include(r => r.Voluntario).FirstOrDefault(x => x.Id == entity.Id);

                if (recurso is not null)
                {
                    recurso.Nome = entity.Nome;
                    recurso.TipoRecurso = entity.TipoRecurso;
                    recurso.Quantidade = entity.Quantidade;
                    recurso.VoluntarioId = entity.VoluntarioId;

                    _context.Recurso.Update(recurso);
                    _context.SaveChanges();

                    return recurso;
                }

                throw new Exception("Não foi possivel localizar o recurso para seguir com a edição");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public RecursoEntity? ObterPorId(int id)
        {
            var recurso = _context.Recurso
                            .Include(r => r.Voluntario)
                            .FirstOrDefault(r => r.Id == id);

            if (recurso is not null)
                return recurso;

            return null;
        }

        public IEnumerable<RecursoEntity> ObterTodos()
        {
            var recursos = _context.Recurso
                .Include(r => r.Voluntario)
                .ToList();

            if (recursos.Any())
                return recursos;

            return Enumerable.Empty<RecursoEntity>();
        }

        public RecursoEntity? SalvarDados(RecursoEntity entity)
        {
            try
            {
                _context.Recurso.Add(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível cadastrar o recurso");
            }
        }
    }
}
