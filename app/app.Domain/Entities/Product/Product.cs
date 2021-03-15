namespace app.Domain.Entities.Product
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string Slug { get; set; }
    }
}
