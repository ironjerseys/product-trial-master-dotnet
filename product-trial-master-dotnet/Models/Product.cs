namespace product_trial_master_dotnet.Models;

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ShellId { get; set; }
    public string InternalReference { get; set; }
    public int Quantity { get; set; }
    public enum InventoryStatus
    {
        INSTOCK,
        LOWSTOCK,
        OUTOFSTOCK
    }
    public double rating { get; set; }

}
