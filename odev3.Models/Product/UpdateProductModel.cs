namespace odev3.Models.Product
{
    public class UpdateProductModel : IProductModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public decimal Price { get; set; }
    }
}