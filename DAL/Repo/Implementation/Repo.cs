using DAL.DataBase;
using DAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repo.Implementation
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<T> dbset;
        public Repo(ApplicationDbContext Context)
        {
            context = Context;
            dbset = context.Set<T>();
        }
        public async Task<(bool, string)> Add(T entity)
        {
            try
            {
                if (entity is null)
                    throw new Exception("Entity cannot be null");
                     var result = await dbset.AddAsync(entity);
                      await context.SaveChangesAsync();
                      return (true, "entity added successfully");
            }
             catch (Exception ex)
            {
                return (false, $"Error {ex.Message}");
            }
        }

        public async Task<(bool, string)> AddRange(List<T> values)
        {
            try
            {
                if (values is null) throw new Exception("Values can't be null");
                await dbset.AddRangeAsync(values);
                await context.SaveChangesAsync();
                return (true,"Items Added Successfully");
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        public async Task<(bool, string)> Delete(long id)
        {
            try
            {
                var result = await dbset.FindAsync(id);
                if (result == null) throw new Exception("Element Not Found!");
                dbset.Remove(result);
                await context.SaveChangesAsync();
                return (true, "Element Deleted successfully!");
            }
            catch (Exception ex)
            {
                return (false, $"Error {ex.Message}");
            }
        }

        public async Task<(bool, string)> Edit(T entity)
        {
            try
            {
                if (entity is null)
                    return (false, "Entity cannot be null");

                dbset.Update(entity); 
                await context.SaveChangesAsync();

                return (true, "Entity updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool, string)> EditRange(List<T> entity)
        {
            try
            {
                if (entity is null)
                    return (false, "Entity cannot be null");

                dbset.UpdateRange(entity);
                await context.SaveChangesAsync();

                return (true, "Entity updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<List<T>> Get(Expression<Func<T,bool>>? predicate = null)
        {
            if (predicate is null)
            {
                var result = await dbset.ToListAsync();
                return (result);
            }
            var result2 =await dbset.Where(predicate).ToListAsync();
            return (result2);
        }
    }
}
