using Ardalis.Specification;

namespace BlazorHomeSite.Data.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}