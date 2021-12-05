using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DataAccess;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.ValueObjects;

namespace ProjectManagement.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override User Add(User user)
        {
            var entity = context.Add(user).Entity;
            context.SaveChanges();
            return entity;

        }

        public override IEnumerable<User> All()
        {
            return context.Users.AsNoTracking().ToList();
        }

        public override User Get(string id)
        {
            return context.Users.Where(t => t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public override User Update(User entity)
        {
            var user = context.Users.AsNoTracking()
                .FirstOrDefault(p => p.Id == entity.Id);
            if (user != null)
            {
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.Password = entity.Password;
                user.Email = Email.From(entity.Email);
                var updatedentity = context.Users.Update(entity).Entity;
                context.SaveChanges();
                return updatedentity;
            }
            else
            {
                return null;
            }
        }

        public override User Delete(string id)
        {
            var user = context.Users.AsNoTracking()
            .FirstOrDefault(p => p.Id == id);
            var entity = context.Users.Remove(user).Entity;
            context.SaveChanges();
            return entity;
        }
    }
}