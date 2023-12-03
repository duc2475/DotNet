using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblTopCategory
    {
        public TblTopCategory()
        {
            TblMidCategories = new HashSet<TblMidCategory>();
        }

        public int TcatId { get; set; }
        public string TcatName { get; set; } = null!;
        public int? ShowOnMenu { get; set; }

        public virtual ICollection<TblMidCategory> TblMidCategories { get; set; }
    }
}
