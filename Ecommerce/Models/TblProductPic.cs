using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblProductPic
    {
        public int PicId { get; set; }
        public string PicName { get; set; } = null!;
        public int? ProductId { get; set; }

        public virtual TblProduct? Product { get; set; }
    }
}
