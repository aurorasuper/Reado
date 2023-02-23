using System.ComponentModel.DataAnnotations;
namespace Read_do.Models
{
    public class ListDetails
    {
        [Key]
        [Required]
        [Display(Name = "Id")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [Display(Name = "Book isbn")]
        public string isbn { get; set; }

        [Display(Name = "Read")]
        public Boolean isRead { get; set; }

        [Display(Name = "Score")]
        public int score { get; set; }
        

        public ListDetails() { }
    }
}
