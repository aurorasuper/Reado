using System.ComponentModel.DataAnnotations;

namespace Read_do.Models
{
    public class BookDetails
    {
        [Key]
        [Required]
        [Display(Name = "ISBN")]
        public string isbn { get; set; }

        
        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Author")]
        public string author { get; set; }

        [Display(Name = "Year")]
        public int publishedYear { get; set; }

        [Display(Name = "Publisher")]
        public string publisher { get; set; }
        public string imageUrl { get; set; }

        public BookDetails() { }
    }
}
