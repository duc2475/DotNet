using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblPromotion
    {
        public TblPromotion()
        {
            TblProductsPromotions = new HashSet<TblProductsPromotion>();
        }

        public int PromoId { get; set; }
        public string PromoName { get; set; } = null!;
        public int PromoDiscount { get; set; }
        public string PromoSdate { get; set; } = null!;
        public string PromoEdate { get; set; } = null!;

        public virtual ICollection<TblProductsPromotion> TblProductsPromotions { get; set; }
    }
}
