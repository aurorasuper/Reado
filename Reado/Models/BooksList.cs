using System;
using System.Collections.Generic;

namespace Reado.Models
{
    public partial class BooksList
    {
        public BooksList()
        {
            TblLists = new HashSet<TblList>();
        }

        public string Isbn { get; set; } = null!;
        public string BookTitle { get; set; } = null!;
        public string BookAuthor { get; set; } = null!;
        public short? YearOfPublication { get; set; }
        public string Publisher { get; set; } = null!;
        public string? ImageUrlS { get; set; }
        public string? ImageUrlM { get; set; }
        public string? ImageUrlL { get; set; }
        public string? Column9 { get; set; }
        public string? Column10 { get; set; }
        public string? Column11 { get; set; }

        public virtual ICollection<TblList> TblLists { get; set; }
    }
}
