namespace BethanysPieShop.API.Entities;

public class ShippingInformation
{
    public int Id { get; set; }

    public string FirstName { get; set; } = default!;
    
    public string LastName { get; set; } = default!;
    
    public string AddressLine1 { get; set; } = default!;
    
    public string ZipCode { get; set; } = default!;
    
    public string City { get; set; } = default!;
    
    public string State { get; set; } = default!;
    
    public string Country { get; set; } = default!;
    
    public string Email { get; set; } = default!;
    
    public string PhoneNumber { get; set; } = default!;
}