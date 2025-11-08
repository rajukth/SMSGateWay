using System.Linq.Expressions;
using SMSGateway.Base.GenericRepository.Interface;
using Microsoft.EntityFrameworkCore;
using SMSGateway.Base.Exceptions;

namespace SMSGateway.Base.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public T Find(long id) => _dbSet.Find(id);

    public async Task<T> FindAsync(long id) => await _dbSet.FindAsync(id);

    public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
    {
        predicate ??= x => true;
        return _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T> GetItemAsync(Expression<Func<T, bool>> predicate) =>
        await _context.Set<T>().FirstOrDefaultAsync(predicate);

    public List<T> Get(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate).ToList();

    public async Task<T> FindOrThrowAsync(long id) => await FindAsync(id);

    public async Task<T> FindOrThrowAsync(Expression<Func<T, bool>> predicate)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(predicate);
        if (entity == null)
        {
            throw new EntityNotFoundException(
                $"Entity of type {typeof(T).Name} with the specified criteria was not found.");
        }

        return entity;
    }

    public IQueryable<T> GetQueryable() => _dbSet.AsQueryable();

    public IQueryable<T> GetBaseQueryable() => _dbSet;

    public Task<bool> CheckIfExistAsync() => _dbSet.AnyAsync();

    public async Task<bool> CheckIfExistAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.AnyAsync(predicate);
}