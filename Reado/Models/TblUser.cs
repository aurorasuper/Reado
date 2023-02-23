using System;
using System.Collections.Generic;

namespace Reado.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblLists = new HashSet<TblList>();
        }

        public int UsrId { get; set; }
        public string UsrName { get; set; } = null!;
        public string UsrEmail { get; set; } = null!;
        public string UsrPassword { get; set; } = null!;

        public virtual ICollection<TblList> TblLists { get; set; }
    }
}
