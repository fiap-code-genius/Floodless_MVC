using Floodless_MVC.Domain.Entities;
using Floodless_MVC.Domain.Interfaces;
using Floodless_MVC.Infrastructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace Floodless_MVC.Infrastructure.Data.Repositories
{
    public class VoluntarioRepository : IVoluntarioRepository
    {
        private readonly ApplicationContext _context;

        public VoluntarioRepository(ApplicationContext context)
        {
            _context = context;
        }

        public VoluntarioEntity? DeletarDados(int id)
        {
            try
            {
                var voluntario = _context.Voluntario.Find(id);

                if (voluntario is not null)
                {
                    _context.Voluntario.Remove(voluntario);
                    _context.SaveChanges();

                    return voluntario;
                }

                throw new Exception("Não foi possivel localizar o voluntário para seguir com a exclusão");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public VoluntarioEntity? EditarDados(VoluntarioEntity voluntario)
        {
            try
            {
                var oVoluntario = _context.Voluntario.FirstOrDefault(x => x.Id == voluntario.Id);

                if (oVoluntario is not null)
                {
                    oVoluntario.Nome = voluntario.Nome;
                    oVoluntario.Email = voluntario.Email;
                    oVoluntario.Contato = voluntario.Contato;

                    _context.Voluntario.Update(oVoluntario);
                    _context.SaveChanges();

                    return oVoluntario;
                }

                throw new Exception("Não foi possivel localizar o voluntário para seguir com a edição");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public VoluntarioEntity? ObterPorId(int id)
        {
            var voluntario = _context.Voluntario.Find(id);

            if (voluntario is not null)
                return voluntario;

            return null;

        }

        public IEnumerable<VoluntarioEntity> ObterTodos()
        {
            var voluntarios = _context.Voluntario.Include(v => v.Recursos).ToList();

            if(voluntarios.Any())
                return voluntarios;

            return null;
        }

        public VoluntarioEntity? SalvarDados(VoluntarioEntity voluntario)
        {
            try
            {
                _context.Voluntario.Add(voluntario);
                _context.SaveChanges();

                return voluntario;
            }
            catch(Exception)
            {
                throw new Exception("Não foi possível cadastrar o voluntário");
            }
        }
    }
}
