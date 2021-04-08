namespace TRMDesktopUI.Library.Models
{
    /// <summary>
    /// This is an Interface of ProductModel class.
    /// </summary>
    public interface IProductModel
    {
        string Description { get; set; }
        int Id { get; set; }
        string ProductName { get; set; }
        int QuantityInStock { get; set; }
        decimal RetailPrice { get; set; }
    }
}