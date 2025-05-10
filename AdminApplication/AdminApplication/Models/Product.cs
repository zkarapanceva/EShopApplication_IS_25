namespace AdminApplication.Models
{
    public class Product : BaseEntity
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
        public int Price { get; set; }
        public int Rating { get; set; }
        public virtual ICollection<ProductInOrder>? ProductsInOrder { get; set; }

    }
}
