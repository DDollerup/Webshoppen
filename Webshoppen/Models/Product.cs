using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshoppen.Models
{
    public class Product
    {
        public Product()
        {
            Image = "no-image.jpg";
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public bool OnSale { get; set; } // true false
        public string SalePrice { get; set; }
        public int CategoryID { get; set; }
    }
}