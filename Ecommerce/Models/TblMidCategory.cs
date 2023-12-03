using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblMidCategory
    {
        public TblMidCategory()
        {
            TblEndCategories = new HashSet<TblEndCategory>();
        }

        public int McatId { get; set; }
        public string McatName { get; set; } = null!;
        public int? TcatId { get; set; }

        public virtual TblTopCategory? Tcat { get; set; }
        public virtual ICollection<TblEndCategory> TblEndCategories { get; set; }
    }
}
