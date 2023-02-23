using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;


namespace Read_do.Models
{
    public class TblUserDetails
    {
        [Key]
        [Required]

        public int Id { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        [StringLength(30, ErrorMessage = "Username can be a maximum of 30 characters.")]
        [Display(Name = "Username")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        [StringLength(150, ErrorMessage = "The password must be at least {2} characters", MinimumLength = 6)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]

        public string password { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(150, ErrorMessage = "E-mail can be a maximum of 150 characters.")]
        [Display(Name = "Email")]
        public string email { get; set; }

        public Boolean isAdmin { get; set; }

        public TblUserDetails() { }
    }
}
