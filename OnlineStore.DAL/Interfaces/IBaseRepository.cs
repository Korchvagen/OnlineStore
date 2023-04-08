namespace OnlineStore.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<bool> Create(T entity);
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;
        Task<bool> Delete(T entity);
        Task<bool> Update(T entity);
    }
}
