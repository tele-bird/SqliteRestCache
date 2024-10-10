using System.ComponentModel.DataAnnotations;

namespace BethanysPieShop.API.Entities;

public class OrderLine
{
    [Key] 
    public long Id { get; set; }
    
    public int PieId { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal TotalPrice { get; set; }

    public Pie? Pie { get; set; }
    
    public long OrderId { get; set; }

    public Order? Order { get; set; }
}