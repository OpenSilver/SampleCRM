namespace SimpleCRM.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
#if OPENSILVER
    using OpenRiaServices.DomainServices.Hosting;
    using OpenRiaServices.DomainServices.Server;
#else
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
#endif


    // The MetadataTypeAttribute identifies studentMetadata as the class
    // that carries additional metadata for the student class.
    [MetadataTypeAttribute(typeof(Category.CategoryMetadata))]
    public partial class Category
    {

        // This class allows you to attach custom attributes to properties
        // of the student class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CategoryMetadata
        {
            // Metadata classes are not meant to be instantiated.
            private CategoryMetadata()
            {
            }

            public int CategoryID { get; set; }
           
            public string Name { get; set; }

            public string Description { get; set; }
        }
    }
}
