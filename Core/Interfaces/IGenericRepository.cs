using System.Linq.Expressions;
using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int Id);
    Task<IEnumerable<T>> GetAllAsync();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression); //Busquedas apartir de expresiones Linq
    Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, string search); //segmentar la cantidad de registros que queremos en una peticion
    void Add(T entity); //agregar 
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity); // Remover
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity); //Actualizar
}