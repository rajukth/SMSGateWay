using SMSGateway.Base.DataContext.Interface;
using Microsoft.EntityFrameworkCore;

namespace SMSGateway.Base.DataContext;

public class Uow : IUow
{
    public DbContext Context { get; }

    public Uow(DbContext context)
    {
        Context = context;
    }

    public void SaveChanges() => Context.SaveChanges();
    public async Task SaveChangesAsync() =>await Context.SaveChangesAsync();

    public void Create<T>(T entity) => Context.Add(entity);
    
    public async Task CreateAsync<T>(T entity) => await Context.AddAsync(entity);

    public void Update<T>(T entity) => Context.Update(entity);

    public void Remove<T>(T entity) => Context.Remove(entity);
    
    public void CreateMultiple<T>(IEnumerable<T> entities) => Context.AddRange(entities);
    
    public async Task CreateMultipleAsync<T>(IEnumerable<T> entities) => await Context.AddRangeAsync(entities);
}