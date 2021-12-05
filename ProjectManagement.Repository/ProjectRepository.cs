using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DataAccess;
using ProjectManager.Domain.Entities;

namespace ProjectManagement.Repository
{
    public class ProjectRepository : GenericRepository<Project>
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override Project Add(Project project)
        {
            var entity = context.Add(project).Entity;
            context.SaveChanges();
            return entity;
        }

        public override IEnumerable<Project> All()
        {
            return context.Projects.AsNoTracking().ToList();
        }

        public override Project Get(string id)
        {
            return context.Projects.Where(t => t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public override Project Update(Project entity)
        {
            var project = context.Projects.AsNoTracking()
                .FirstOrDefault(p => p.Id == entity.Id);
            if (project != null)
            {
                project.Name = entity.Name;
                project.Detail = entity.Detail;

                var updatedentity = context.Projects.Update(project).Entity;
                context.SaveChanges();
                return updatedentity;
            }
            else
            {
                return null;
            }
        }

        public override Project Delete(string id)
        {
            var project = context.Projects.AsNoTracking()
           .FirstOrDefault(p => p.Id == id);
            var entity = context.Projects.Remove(project).Entity;
            context.SaveChanges();
            return entity;
        }
    }
}
