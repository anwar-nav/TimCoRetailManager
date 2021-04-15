using System.Collections.Generic;
using TRMDataManger.Library.Models;

namespace TRMDataManger.Library.DataAccess
{
    public interface IProductData
    {
        List<ProductModel> GetProducts();
        ProductModel GetProductsById(int productId);
    }
}