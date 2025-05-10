namespace AdminApplication.Models
{
    public class Order : BaseEntity
    {
        public string? OwnerId { get; set; }
        public EShopApplicationUser Owner { get; set; }
        public virtual ICollection<ProductInOrder>? ProductsInOrder { get; set; }

    }
}
