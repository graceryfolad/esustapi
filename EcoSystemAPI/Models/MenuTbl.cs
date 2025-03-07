using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esust.Models
{
    [Table("Menustbl")]
    public class MenuTbl
    {
        [Key, Column("mnu_id")]
        public int Id { get; set; }
        [Column("mnu_created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("mnu_label")]
        public string Label { get; set; }
        [Column("mnu_type")]
        public string MenuType { get; set; }
        [Column("mnu_isvisible")]
        public bool IsVisible { get; set; }
        [Column("mnu_order")]
        public int MenuOrder { get; set; }
        [Column("mnu_level")]
        public int level { get; set; }
    }

    public class CreateMenu
    {
       
        
        public string Label { get; set; }
       
        public string MenuType { get; set; }
       
       
        public int level { get; set; }
    }
}
