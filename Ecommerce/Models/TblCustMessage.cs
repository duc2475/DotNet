using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblCustMessage
    {
        public int CustMsgId { get; set; }
        public string SubjectMsg { get; set; } = null!;
        public string Msg { get; set; } = null!;
        public int CustId { get; set; }
    }
}
