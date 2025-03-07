using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esust.Models
{
    [Table("Pagestbl")]
    public class PageTbl
    {
        [Key, Column("pg_id")]
        public int Id { get; set; }
        [Column("pg_created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("pg_content")]
        public string Description { get; set; }
        [Column("pg_title")]
        public string Title { get; set; }
        [Column("pg_modified_date")]
        public DateTime? ModifiedDate { get; set; }
        [Column("pg_menu_id")]
        public int MenuID { get; set; }
    }
}
