using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblCart
    {
        public TblCart()
        {
            TblCartDetails = new HashSet<TblCartDetail>();
            TblOrders = new HashSet<TblOrder>();
        }

        public int CartId { get; set; }
        public string CartDate { get; set; } = null!;
        public string TotalPrice { get; set; } = null!;
        public int CustId { get; set; }

        public virtual TblCustomer Cust { get; set; } = null!;
        public virtual ICollection<TblCartDetail> TblCartDetails { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
