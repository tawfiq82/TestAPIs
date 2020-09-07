using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestAPIs.Models
{
    public class TrolleyItems
    {
        [Required]
        public IList<TrolleyProduct> Products { get; set; }

        [Required]
        public IList<TrolleyQuantity> Quantities { get; set; }

        [Required]
        public IList<SpecielItems> Specials { get; set; }
    }

    public class TrolleyProduct
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }

    public class TrolleyQuantity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }
    }

    public class SpecielItems
    {
        [Required]
        public IList<TrolleyQuantity> Quantities { get; set; }

        [Required]
        public decimal Total { get; set; }
    }
}
