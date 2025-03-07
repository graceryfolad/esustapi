using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoSystemAPI.Models
{
    public class Login
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    [Table("users")]
    public class UserModel
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usr_firstname")]
        public string Firstname { get; set; }
        [Column("usr_lastname")]
        public string Lastname { get; set; }

        [Column("usr_email")]
        public string Email { get; set; }

        [Column("usr_password")]
        public string Password { get; set; }

        [Column("usr_status")]
        public bool Status { get; set; }

        [Column("usr_last_login")]
        public DateTime? LastLogin { get; set; }

        [Column("usr_created")]
        public DateTime CreatedDate { get; set; }

        [Column("usr_type")]
        public int UserType { get; set; }

        [Column("usr_bvn")]
        public string? BVN { get; set; }

        [Column("usr_phone_number")]
        public string PhoneNumber { get; set; }

        [Column("usr_salt")]
        public string Salt { get; set; }
        [Column("usr_updated")]
        public DateTime? updated_at { get; set; }
        public bool isactive { get; set; }
    }

    

    public class CreateUser 
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

    }

    public class AccountLoginResponse
    {
        public int AccountID { get; set; }
        public string Username { get; set; }
        public string ErrorMessage { get; set; }
    }
}
