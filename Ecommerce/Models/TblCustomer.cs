using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblCustomer
    {
        public TblCustomer()
        {
            TblCarts = new HashSet<TblCart>();
            TblOrders = new HashSet<TblOrder>();
            TblUserCustomers = new HashSet<TblUserCustomer>();
        }

        public int CustId { get; set; }
        public string CustName { get; set; } = null!;
        public string CustLname { get; set; } = null!;
        public string CustEmail { get; set; } = null!;
        public string CustPhone { get; set; } = null!;
        public string CustAddress { get; set; } = null!;
        public string CustCity { get; set; } = null!;
        public string CustSate { get; set; } = null!;
        public string CustZip { get; set; } = null!;
        public string CustDatetime { get; set; } = null!;
        public int CustStatus { get; set; }

        public virtual ICollection<TblCart> TblCarts { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
        public virtual ICollection<TblUserCustomer> TblUserCustomers { get; set; }
    }
}
