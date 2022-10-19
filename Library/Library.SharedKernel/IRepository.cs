using System.Linq.Expressions;

namespace Library.SharedKernel;

public interface IRepository<T> where T : Entity
{
    /// <summary>
    ///     Filter:
    ///     The code Expression<Func
    ///     <T, bool>
    ///         > filter means the caller will provide a lambda expression based on the T type,
    ///         and this expression will return a Boolean value. For example, if the repository is instantiated for the Student
    ///         entity type, the code in the calling method might specify student => student.LastName == "Smith" for the filter
    ///         parameter.
    ///         OrderBy:
    ///         The code Func<IQueryable
    ///         <TEntity>
    ///             , IOrderedQueryable
    ///             <TEntity>
    ///                 > orderBy also means the caller will provide a
    ///                 lambda expression.But in this case,the input to the expression is an IQueryable object for the TEntity
    ///                 type.
    ///                 The expression will return an ordered version of that IQueryable object.the repository is instantiated
    ///                 for the
    ///                 Student code in the calling method might specify q => q.OrderBy(s => s.LastName) for the orderBy
    ///                 parameter.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="filter">see summary</param>
    /// <param name="orderBy">see summary</param>
    /// <param name="orderByDesc"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, string>>? orderBy = null,
        bool? orderByDesc = false);

    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task CreateAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}