namespace Sales.Models.Catalog
{
   
    public class MenuItemModel
    { 
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Active { get; set; } = string.Empty;
    }
}
