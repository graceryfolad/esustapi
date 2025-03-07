using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace esust.Models
{
    public class EventTbl
    {
        [Key, Column("ev_id")]
        public int Id { get; set; }
        [Column("ev_title", TypeName = "varchar"), MaxLength(300), MinLength(10)]
        public string Title { get; set; }
        [Column("ev_content"), MinLength(50)]
        public string Body { get; set; }
        [Column("ev_imageurl")]
        public string ImageUrl { get; set; }
        [Column("ev_created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("ev_event_date")]
        public DateTime EventDate { get; set; }
        [Column("ev_modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
