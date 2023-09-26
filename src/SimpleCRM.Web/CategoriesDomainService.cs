namespace SimpleCRM.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
#if OPENSILVER
    using OpenRiaServices.DomainServices.EntityFramework;
    using OpenRiaServices.DomainServices.Hosting;
    using OpenRiaServices.DomainServices.Server;
#else
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
#endif

    [EnableClientAccess()]
    public class CategoriesDomainService : LinqToEntitiesDomainService<WCFriadbEntities>
    {
        public IQueryable<Category> GetCategories()
        {
            //var retval = new Category[] { new Category { CategoryID = 1, Description = "Desc", Name = "MyCategory" } };
            //var qretval = retval.AsQueryable();
            //return qretval;
            return ObjectContext.Categories.OrderBy(s => s.Name);
        }

        public IQueryable<Category> GetCategoryByID(int id)
        {
            return ObjectContext.Categories.Where(s => s.CategoryID == id);
        }

        public void InsertCategory(Category category)
        {
            if ((category.EntityState != EntityState.Detached))
            {
                ObjectContext.ObjectStateManager.ChangeObjectState(category, EntityState.Added);
            }
            else
            {
                ObjectContext.Categories.AddObject(category);
                ObjectContext.SaveChanges();
            }
        }

        public void UpdateCategory(Category currentCategory)
        {
            ObjectContext.Categories.AttachAsModified(currentCategory, ChangeSet.GetOriginal(currentCategory));
            ObjectContext.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            if ((category.EntityState != EntityState.Detached))
            {
                ObjectContext.ObjectStateManager.ChangeObjectState(category, EntityState.Deleted);
            }
            else
            {
                ObjectContext.Categories.Attach(category);
                ObjectContext.Categories.DeleteObject(category);
                ObjectContext.SaveChanges();
            }
        }
    }
}