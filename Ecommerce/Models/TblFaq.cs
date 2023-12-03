using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public partial class TblFaq
    {
        public int FaqId { get; set; }
        public string? FaqTitle { get; set; }
        public string FaqContent { get; set; } = null!;
    }
}
