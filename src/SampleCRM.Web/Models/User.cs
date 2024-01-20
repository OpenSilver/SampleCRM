using OpenRiaServices.DomainServices.Server.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Web
{
    /// <summary>
    /// Class containing information about the authenticated user.
    /// </summary>
    public partial class User : IUser
    {
        //// NOTE: Profile properties can be added for use in application.
        //// To enable profiles, edit the appropriate section of web.config file.
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// Gets and sets the friendly name of the user.
        /// </summary>
        public string FriendlyName { get; set; }

        [Key]
        public string Name { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}