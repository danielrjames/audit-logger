using app.Domain.Entities.Product;

namespace app.Web.ViewModels
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Price { get; set; }
        public string Slug { get; set; }

        public ProductDetailsVM(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Color = product.Color;
            Price = string.Format("{0:C}", product.Price);
            Slug = product.Slug;
        }
    }
}
