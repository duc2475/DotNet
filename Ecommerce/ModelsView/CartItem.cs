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
        public double totalMoney => amount * Product.ProductPrice.Value;
    }
}
