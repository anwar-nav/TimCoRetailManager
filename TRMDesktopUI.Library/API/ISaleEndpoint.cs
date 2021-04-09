using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
    /// <summary>
    /// This is an Interface for SaleEndpoint class.
    /// </summary>
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}