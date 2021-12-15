using System.ComponentModel.DataAnnotations;

namespace odev3.Models.Product
{
    public class ProductViewModel : IProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


    }
}