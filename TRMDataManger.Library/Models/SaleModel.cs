using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManger.Library.Models
{
    /// <summary>
    /// This will have the details of cart received from UI. Not instantiating here because it will be checked in controller.
    /// </summary>
    public class SaleModel
    {
        public List<SaleDetailModel> SaleDetails { get; set; }
    }
}
