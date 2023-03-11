using Ardalis.Specification;

namespace BlazorHomeSite.Data.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}