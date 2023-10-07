using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaisRepository : GenericRepository<Pais>, IPaisRepository
    {
        private readonly AnimalsContext _context;

        public PaisRepository(AnimalsContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Pais>> GetAllAsync()
        {
            return await _context.Paises
                .Include(x => x.Departamentos)
                .ThenInclude(x => x.Ciudades)
                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Pais> registros)> GetAllAsync( //Sobrecarga de metodos
            int pageIndex, //Cual pagina necesitamos ver
            int pageSize, //Cantidad de registros a visualizar por pagina
            string search // Pasar algun critetio de busqueda
        )
        {
            var query = _context.Paises as IQueryable<Pais>; //Consulta para obtener todos los registros en este caso paises
            if (!string.IsNullOrEmpty(search)) //nos permite referenciar si esta variable es nula 
            {
                query = query.Where(p => p.NombrePais.ToLower().Contains(search)); //si no esta vacia ejecuta la busqueda mediante el nombre del pais 
            }
            query = query.OrderBy(p => p.Id); //ordena el listado por el id
            var totalRegistros = await query.CountAsync(); //obtiene el total de registros
            var registros = await query // obtiene los registros
                .Include(u => u.Departamentos)
                .Skip((pageIndex-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (totalRegistros, registros);
        }
    }
}
