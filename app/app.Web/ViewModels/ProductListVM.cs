using System.Collections.Generic;

namespace app.Web.ViewModels
{
    public class ProductListVM
    {
        public List<ProductVM> Products { get; set; }

        public ProductListVM()
        {
            Products = new List<ProductVM>();
        }
    }
}
