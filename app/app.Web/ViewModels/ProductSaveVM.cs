using app.Domain.Entities.Product;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace app.Web.ViewModels
{
    public class ProductSaveVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Color { get; set; }
        [Required]
        public decimal Price { get; set; }
        public List<ColorOptionVM> ColorList { get; set; }
        public string ReturnUrl { get; set; }
        public bool Error { get; set; }

        public ProductSaveVM()
        {
            ReturnUrl = "/products";
        }

        public ProductSaveVM(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            ReturnUrl = string.Format("/products/{0}", product.Slug);
        }
    }
}
