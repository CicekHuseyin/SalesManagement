using Microsoft.EntityFrameworkCore;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;
using System.Linq.Expressions;

namespace SalesManagement.Data.Concretes;

/// <summary>
/// Tüm entity'ler için ortak CRUD işlemlerini sağlayan generic repository.
/// </summary>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TestDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(TestDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IEnumerable<T> GetAll() => _dbSet.ToList();

    public T? GetById(int id) => _dbSet.Find(id);

    public void Add(T entity) => _dbSet.Add(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);

    public void Save() => _context.SaveChanges();

    public T Get(Expression<Func<T, bool>> filter) => _dbSet.FirstOrDefault(filter)!;

    public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter) => _dbSet.Where(filter).ToList();

    public IQueryable<T> Query() => _dbSet.AsQueryable();
}
