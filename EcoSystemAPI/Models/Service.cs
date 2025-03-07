namespace EcoSystemAPI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("services")]
    public class ServiceModel
    {
        [Key, Column("ser_code")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Since auto-increment is false
        public string ServiceCode { get; set; }

        [Required]
        [Column("ser_name")]
        public string ServiceName { get; set; }

        [Required]
        [Column("ser_status")]
        public bool ServiceStatus { get; set; }
    }

}
