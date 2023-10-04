using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Web.Models
{
    public class CountModel
    {
        [Key]
        public int Id { get; set; }
        public int CategoryCount { get; set; }
        public int CustomerCount { get; set; }
        public int OrderCount { get; set; }
        public int ProductCount { get; set; }
    }
}