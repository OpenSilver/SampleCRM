
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
    public class StudentDomainService : LinqToEntitiesDomainService<WCFriadbEntities>
    {
        public IQueryable<student> GetStudents()
        {
            return this.ObjectContext.students.OrderBy(s=>s.StudentName);
        }

        public IQueryable<student> GetStudentsByID(int id)
        {
            return this.ObjectContext.students.Where(s => s.ID == id);
        }

        public void InsertStudent(student student)
        {
            if ((student.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(student, EntityState.Added);
            }
            else
            {
                this.ObjectContext.students.AddObject(student);
                this.ObjectContext.SaveChanges();
            }
        }

        public void UpdateStudent(student currentstudent)
        {
            this.ObjectContext.students.AttachAsModified(currentstudent, this.ChangeSet.GetOriginal(currentstudent));
            this.ObjectContext.SaveChanges();
        }

        public void DeleteStudent(student student)
        {
            if ((student.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(student, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.students.Attach(student);
                this.ObjectContext.students.DeleteObject(student);
                this.ObjectContext.SaveChanges();
            }
        }
    }
}


