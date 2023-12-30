using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;

namespace Ecommerce.ModelsView
{
    public class CartItem
    {
        public TblProduct Product { get; set; }
        public int amount { get; set; }
        public double totalMoney => Product.TblProductsPromotions.Count()!=0? amount * (Product.ProductPrice.Value - (Product.ProductPrice.Value * Convert.ToDouble(Product.TblProductsPromotions.FirstOrDefault(x=> x.ProductId == Product.ProductId).Promo.PromoDiscount) /100)) :amount * Product.ProductPrice.Value;
    }
}
