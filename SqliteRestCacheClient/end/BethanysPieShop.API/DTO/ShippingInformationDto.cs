namespace BethanysPieShop.API.DTO;

public class ShippingInformationDto
{
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