using Microsoft.EntityFrameworkCore;

namespace SMSGateway.Base.DataContext.Interface;

public interface IUow
{
    DbContext Context { get; }
    void SaveChanges();
    Task SaveChangesAsync();
    Task CreateAsync<T>(T entity);
    void Create<T>(T entity);
    void Update<T>(T entity);
    void Remove<T>(T entity);
    
    Task CreateMultipleAsync<T>(IEnumerable<T> entities);
    void CreateMultiple<T>(IEnumerable<T> entities);
    
}