using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblCartDetail
    {
        public int CartDetailId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductPic { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int Quantity { get; set; }

        public virtual TblCart Cart { get; set; } = null!;
        public virtual TblProduct Product { get; set; } = null!;
    }
}
