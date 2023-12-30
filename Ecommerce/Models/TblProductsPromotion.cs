using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblProductsPromotion
    {
        public int PromoId { get; set; }
        public int ProductId { get; set; }
        public int PpId { get; set; }

        public virtual TblProduct Product { get; set; } = null!;
        public virtual TblPromotion Promo { get; set; } = null!;
        
        public int getdiscount() { return Promo.PromoDiscount; }
    }
}
