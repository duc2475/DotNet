using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblProduct
    {
        public TblProduct()
        {
            TblCartDetails = new HashSet<TblCartDetail>();
            TblProductPics = new HashSet<TblProductPic>();
            TblProductsPromotions = new HashSet<TblProductsPromotion>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductPic { get; set; }
        public string Color { get; set; } = null!;
        public int StockQuantity { get; set; }
        public int EcatId { get; set; }
        public string? ProductDes { get; set; }
        public string? ProductStatus { get; set; }
        public int? ProductPrice { get; set; }
        public string? ProductSeo { get; set; }

        public virtual TblEndCategory Ecat { get; set; } = null!;
        public virtual ICollection<TblCartDetail> TblCartDetails { get; set; }
        public virtual ICollection<TblProductPic> TblProductPics { get; set; }
        public virtual ICollection<TblProductsPromotion> TblProductsPromotions { get; set; }
    }
}
