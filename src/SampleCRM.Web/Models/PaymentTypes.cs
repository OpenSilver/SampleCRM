//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleCRM.Web.Models
{
    using System;using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    
    public partial class PaymentTypes
    {
        [Key]public long PaymentTypeID { get; set; }
        public string Name { get; set; }
    }
}
