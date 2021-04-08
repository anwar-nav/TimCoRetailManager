using System.Collections.Generic;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
    /// <summary>
    /// This is an Interface of ProductEndpoint class.
    /// </summary>
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}