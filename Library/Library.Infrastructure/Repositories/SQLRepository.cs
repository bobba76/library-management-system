using System.Data.SqlClient;
using System.Linq.Expressions;
using Library.Domain.EmployeeAggregate;
using Library.Domain.EmployeeAggregate.Roles;
using Library.SharedKernel;
using MediatR;

namespace Library.Infrastructure.Repositories;

public abstract class SQLRepository<T> : IRepository<T> where T : Entity
{
    protected readonly string _connectionString = DB.ConnectionString;
    protected readonly IMediator _mediator;

    protected SQLRepository(IMediator mediator)
    {
        _mediator = mediator;
    }

    public abstract Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken, Expression<Func<T, bool>>? filter = null, Expression<Func<T, string>>? orderBy = null);
    public abstract Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public abstract Task CreateAsync(T entity, CancellationToken cancellationToken);
    public abstract Task UpdateAsync(T entity, CancellationToken cancellationToken);
    public abstract Task DeleteAsync(T entity, CancellationToken cancellationToken);
}