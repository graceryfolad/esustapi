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

    }
}
