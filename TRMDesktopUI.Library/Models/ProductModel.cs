﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.Library.Models
{
    /// <summary>
    /// This has the properties of API ProductModel. This is required for mapping it to API ProductModel (The object model
    /// used for storing data).
    /// </summary>
    public class ProductModel : IProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        public int QuantityInStock { get; set; }
    }
}