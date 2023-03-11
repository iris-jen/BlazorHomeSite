using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using BlazorHomeSite.Data.Interfaces;

namespace BlazorHomeSite.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepositoryBase<T>, IRepositoryBase<T> where T : class, IAggregateRoot
{
    public EfRepository(HomeSiteDbContext dbContext) : base(dbContext)
    {
    }
}