using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCRM.Web.Models
{
    public partial class Customers
    {
        public bool IsNew => CustomerID <= 0;
        public string FullName => $"{FirstName} {LastName}";
        public string Initials => String.Format("{0}{1}", $"{FirstName} "[0], $"{LastName} "[0]).Trim().ToUpper();
        public string CountryName { get; set; }
    }
}