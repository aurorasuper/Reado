using System;
using System.Collections.Generic;

namespace Reado.Models
{
    public partial class TblList
    {
        public int ListId { get; set; }
        public int ListUsrId { get; set; }
        public string ListIsbn { get; set; } = null!;
        public bool ListRead { get; set; }
        public int? ListScore { get; set; }

        public virtual BooksList ListIsbnNavigation { get; set; } = null!;
        public virtual TblUser ListUsr { get; set; } = null!;
    }
}
