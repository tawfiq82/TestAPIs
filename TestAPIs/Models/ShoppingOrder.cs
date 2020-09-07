using System.Collections.Generic;

namespace TestAPIs.Models
{
    public class ShoppingOrder
    {
        public int CustomerId { get; set; }

        public IList<Product> Products { get; set; }
    }
}
