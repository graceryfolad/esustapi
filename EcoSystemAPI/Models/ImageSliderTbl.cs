using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esust.Models
{
    [Table("SliderImages")]
    public class ImageSliderTbl
    {
        [Key,Column("si_id")]
        public int Id { get; set; }
        [Column("si_title",TypeName ="varchar"), MaxLength(50)]
        public string Title { get; set; }
        [Column("si_desc", TypeName = "varchar"), MaxLength(150)]
        public string Description { get; set; }
        [Column("si_action", TypeName = "varchar"), MaxLength(500)]
        public string LinkAction { get; set; }
        [Column("si_image", TypeName = "varchar"), MaxLength(500)]
        public string Image { get; set; }

    }

    public class CreateSlider
    {
        [Required, MaxLength(50)]
        public string Title { get; set; }
       
        public string Description { get; set; }
       
        public string LinkAction { get; set; }
        [Required]
        public IFormFile Image { get; set; }

    }
}
