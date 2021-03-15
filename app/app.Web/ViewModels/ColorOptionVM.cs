using app.Domain.Entities.Product;

namespace app.Web.ViewModels
{
    public class ColorOptionVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ColorOptionVM(ColorOption colorOption)
        {
            Id = colorOption.Id;
            Name = colorOption.Name;
        }
    }
}
