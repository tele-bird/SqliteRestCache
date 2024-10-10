namespace BethanysPieShop.Mobile.Services.Interfaces;

public interface INavigationService
{
    Task GoBack();
    
    Task GoTo(string route);
    
    Task GoToLogin(string? returnUrl);
    
    Task GoToRegister(string? returnUrl);
    
    Task GoToSelectedOrderDetail(long selectedOrderId);
    
    Task GoToSelectedPieDetail(int selectedPieId);
}