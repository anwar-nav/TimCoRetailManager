using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    /// <summary>
    /// This is for storing the details of items in cart for sending to API
    /// </summary>
    public class SaleDetailModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
