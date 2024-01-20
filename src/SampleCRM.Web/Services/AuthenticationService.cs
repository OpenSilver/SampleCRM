using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using OpenRiaServices.DomainServices.Server.Authentication;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace SampleCRM.Web
{
    // TODO: Switch to a secure endpoint when deploying the application.
    //       The user's name and password should only be passed using https.
    //       To do this, set the RequiresSecureEndpoint property on EnableClientAccessAttribute to true.
    //
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       More information on using https with a Domain Service can be found on MSDN.

    /// <summary>
    /// Domain Service responsible for authenticating users when they log on to the application.
    ///
    /// Most of the functionality is already provided by the AuthenticationBase class.
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : DomainService, IAuthentication<User>
    {
        private static readonly User DefaultUser = new User
        {
            Name = string.Empty
        };

        /// <summary>
        /// Use this method to fill your User object with additional data (from database, for example)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private User MapMembershipUser(MembershipUser user)
        {
            return new User
            {
                Name = user.UserName
            };
        }

        public bool ValidateUser(string userName, string password) => Membership.ValidateUser(userName, password);

        [Query(IsComposable = false)]
        public User GetUser()
        {
            IIdentity identity = null;
            try
            {
                identity = ServiceContext?.User?.Identity;
            }
            catch (InvalidOperationException)
            { 
                
            }

            if (identity is null) return DefaultUser;

            if (identity.IsAuthenticated)
            {
                var user = Membership.GetUser(identity.Name);

                return MapMembershipUser(user);
            }

            return DefaultUser;
        }

        public User Login(string userName, string password, bool isPersistent, string customData)
        {
            if (!ValidateUser(userName, password)) return default;

            // if IsPersistent is true, will keep logged in for up to a week (or until you logout)
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: userName,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(60),
                isPersistent,
                userData: customData ?? string.Empty,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = false,
                Expires = ticket.Expiration
            };

            var httpContext = (HttpContextBase)ServiceContext.GetService(typeof(HttpContextBase));
            httpContext.Response.Cookies.Add(authCookie);

            var user = Membership.GetUser(userName);

            if (user == null) return default;

            return MapMembershipUser(user);
        }

        public User Logout()
        {
            FormsAuthentication.SignOut();
            return DefaultUser;
        }

        public void UpdateUser(User user)
        {
        }
    }
}