namespace WebIdentityApi.DTOs.ProductVariant
{
    public class CreateProductVariantDto
    {
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        
    }
}
