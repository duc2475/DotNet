using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblUserCustomer
    {
        public int CustId { get; set; }
        public int UserId { get; set; }

        public virtual TblCustomer Cust { get; set; } = null!;
        public virtual TblUser User { get; set; } = null!;
    }
}
