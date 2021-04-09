using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManger.Library.Models
{
    /// <summary>
    /// This has the properties of Sale Detail Table (same as column names of Sale Detail Table).
    /// </summary>
    public class SaleDetailDBModel
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Tax { get; set; }
    }
}
