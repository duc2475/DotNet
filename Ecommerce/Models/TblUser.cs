using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblUserCustomers = new HashSet<TblUserCustomer>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        public string UserPass { get; set; } = null!;
        public string UserPhoto { get; set; } = null!;
        public string UserRole { get; set; } = null!;
        public string UserStatus { get; set; } = null!;

        public virtual ICollection<TblUserCustomer> TblUserCustomers { get; set; }
    }
}
