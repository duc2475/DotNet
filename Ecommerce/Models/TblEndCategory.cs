using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblEndCategory
    {
        public TblEndCategory()
        {
            TblProducts = new HashSet<TblProduct>();
        }

        public int EcatId { get; set; }
        public string EcatName { get; set; } = null!;
        public int? McatId { get; set; }

        public virtual TblMidCategory? Mcat { get; set; }
        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
