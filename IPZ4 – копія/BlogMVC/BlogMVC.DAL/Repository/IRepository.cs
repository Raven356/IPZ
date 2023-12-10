namespace BlogMVC.DAL.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> Add(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(int? id);

        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById(object id);
    }
}
