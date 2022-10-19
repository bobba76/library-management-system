using System.Linq.Expressions;
using Library.SharedKernel;

namespace Library.Infrastructure.Repositories;

public abstract class SQLRepository<T> : IRepository<T> where T : Entity
{
    protected readonly string _connectionString = DB.ConnectionString;

    public abstract Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filter = null, Expression<Func<T, string>>? orderBy = null,
        bool? orderByDesc = false);

    public abstract Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
    public abstract Task CreateAsync(T entity, CancellationToken cancellationToken);
    public abstract Task UpdateAsync(T entity, CancellationToken cancellationToken);
    public abstract Task DeleteAsync(T entity, CancellationToken cancellationToken);
}