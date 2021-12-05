using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagement.DataAccess;

namespace ProjectManagement.Repository
{
    public abstract class GenericRepository<T>
        : IRepository<T> where T : class
    {
        protected ApplicationDbContext context;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public virtual T Add(T entity)
        {
            return context
                .Add(entity)
                .Entity;
        }
        
        public virtual T Get(string id)
        {
            return context.Find<T>(id);
        }

        public virtual IEnumerable<T> All()
        {
            return context.Set<T>()
                .ToList();
        }

        public virtual T Update(T entity)
        {
            return context.Update(entity)
                .Entity;
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }

        public virtual T Delete(string id)
        {
            return context.Remove(context.Find<T>(id)).Entity;
        }
    }
}
