using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Web.Attributes
{
    public class RestrictAccessDeveloperMode : AuthorizationAttribute
    {
        protected override AuthorizationResult IsAuthorized(
            System.Security.Principal.IPrincipal principal, AuthorizationContext authorizationContext)
        {
            //EmployeePayHistory eph = (EmployeePayHistory)authorizationContext.Instance;
            //Employee selectedEmployee;
            //Employee authenticatedUser;

            //using (AdventureWorksEntities context = new AdventureWorksEntities())
            //{
            //    selectedEmployee = context.Employees.SingleOrDefault(e => e.EmployeeID == eph.EmployeeID);
            //    authenticatedUser = context.Employees.SingleOrDefault(e => e.LoginID == principal.Identity.Name);
            //}

            //if (selectedEmployee.ManagerID == authenticatedUser.EmployeeID)
            //{
            //    return AuthorizationResult.Allowed;
            //}
            //else
            //{
            //    return new AuthorizationResult("Only the authenticated manager for the employee can add a new record.");
            //}

            //TODO: Implement a proper Auth and Role Check

            if (Global.DeveloperMode)
            {
                return AuthorizationResult.Allowed;
            }
            else
            {
                return new AuthorizationResult("DB is readonly for production mode");
            }
        }
    }
}