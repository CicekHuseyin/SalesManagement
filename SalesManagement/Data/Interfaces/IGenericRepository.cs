using System.Linq.Expressions;

namespace SalesManagement.Data.Interfaces;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAll(Expression<Func<T, bool>> filter);
    T Get(Expression<Func<T, bool>> filter);
    T? GetById(int id);
    IQueryable<T> Query();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Save();
}

