using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblOrder
    {
        public int OrderId { get; set; }
        public int CartId { get; set; }
        public int CustId { get; set; }
        public string CustName { get; set; } = null!;
        public string ShipingAddress { get; set; } = null!;
        public string ShipingCity { get; set; } = null!;
        public string TotalPrice { get; set; } = null!;
        public string OrderStatus { get; set; } = null!;

        public virtual TblCart Cart { get; set; } = null!;
        public virtual TblCustomer Cust { get; set; } = null!;
    }
}
