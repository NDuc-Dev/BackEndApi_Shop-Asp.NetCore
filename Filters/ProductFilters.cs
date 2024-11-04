namespace WebIdentityApi.Filters
{
    public class ProductFilters
    {
        public string Name { get; set; }
        public int? Brand { get; set; }
        public int[] Size { get; set; }
        public int[] Color { get; set; }
        public string CreatedByUser { get; set; }
        public bool Status { get; set; } = true;
    }

}