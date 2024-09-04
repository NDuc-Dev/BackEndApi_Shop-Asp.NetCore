namespace WebIdentityApi.DTOs.Admin.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ImagePath { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
