using System.Linq.Expressions;

namespace SMSGateway.Base.GenericRepository.Interface;

public interface IGenericRepository <T> where T : class
{
    T Find(long id);
    Task<T> FindAsync(long id);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
    Task<T> GetItemAsync(Expression<Func<T, bool>> predicate);
    List<T> Get(Expression<Func<T, bool>> predicate);
    Task<T> FindOrThrowAsync(long id);
    Task<T> FindOrThrowAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetQueryable();
    IQueryable<T> GetBaseQueryable();
    Task<bool> CheckIfExistAsync();
    
    Task<bool> CheckIfExistAsync(Expression<Func<T, bool>> predicate);
}