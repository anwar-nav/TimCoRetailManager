﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManger.Library.Models
{
    /// <summary>
    /// This has the properties of Product Table (same as column names of Product Table).
    /// </summary>
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsTaxable { get; set; }
    }
}
