using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("CATEGORY NAME")]
        public string Name { get; set; }

        [DisplayName("DISPLAY ORDER")]
        [Range(1, 100, ErrorMessage = "DISPLAY ORDER MUST BE BETWEEN 1-100")]
        public int DisplayOrder { get; set; }


    }
}
