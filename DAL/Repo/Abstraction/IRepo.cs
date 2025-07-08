using System.Linq.Expressions;

namespace DAL.Repo.Abstraction
{
    public interface IRepo <T> where T :class
    {
        public Task<(bool ,string)> Add(T entity);
        public Task<(bool, string)> Edit(T entity);
        public Task<(bool, string)> EditRange(List<T> entity);
        public Task<(bool, string)> Delete(T entity);
        public Task<List<T>> GetAll(Expression<Func<T,bool>>? Delegate = null);
        public Task<(bool, string)> AddRange(List<T> values);
        public Task<T> Get(Expression<Func<T, bool>> Delegate);
    }
}
