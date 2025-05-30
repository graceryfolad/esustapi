﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esust.Models
{
    [Table("newstbl")]
    public class NewsTbl
    {
        [Key,Column("nw_id")]
        public int Id { get; set; }
        [Column("nw_title", TypeName = "varchar"), MaxLength(300), MinLength(10)]
        public string Title { get; set; }
        [Column("nw_content"), MinLength(50)]
        public string Body { get; set; }
        [Column("nw_imageurl")]
        public string DefaultImageUrl { get; set; }
        [Column("nw_created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("nw_modified_date")]
        public DateTime? ModifiedDate { get; set; }
        [Column("nw_short"), MinLength(50)]
        public string? ShortDescription { get; set; }

    }

    public class AddNews
    {
        [Required, MinLength(10), MaxLength(250)]
        public string Title { get; set; }
        [MinLength(50),Required]
        public string Body { get; set; }
        [Required]
        public IFormFile DefaultImageUrl { get; set; }
        [MaxLength(250), Required]
        public string ShortDescription { get; set; }
    }

    public class EditNews
    {
        [Required, MinLength(10), MaxLength(250)]
        public string Title { get; set; }
        [MinLength(50), Required]
        public string Body { get; set; }
        [Required]
        public int NewsID { get; set; }       
        public string ShortDescription { get; set; }
    }

    public class ChangeImage
    {
       
        public int ID { get; set; }
        [Required]
        public IFormFile ImageUrl { get; set; }
        [Required]
        public string ImageType { get; set; }
    }

    public class DeleteNews
    {
        [Required]
        public int NewsID { get; set; }
    }
}
