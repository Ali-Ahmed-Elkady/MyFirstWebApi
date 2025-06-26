using System.Linq.Expressions;

namespace DAL.Repo.Abstraction
{
    public interface IRepo <T> where T :class
    {
        public Task<(bool ,string)> Add(T entity);
        public Task<(bool, string)> Edit(T entity);
        public Task<(bool, string)> EditRange(List<T> entity);
        public Task<(bool, string)> Delete(long id);
        public Task<List<T>> Get(Expression<Func<T,bool>>? Delegate = null);
        public Task<(bool, string)> AddRange(List<T> values);
    }
}
