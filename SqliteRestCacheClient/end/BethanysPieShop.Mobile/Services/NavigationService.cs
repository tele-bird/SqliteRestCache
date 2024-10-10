using BethanysPieShop.Mobile.Services.Interfaces;

namespace BethanysPieShop.Mobile.Services;

public class NavigationService : INavigationService
{
    public Task GoBack()
        => Navigate("..");

    public Task GoTo(string route)
        => Navigate(route);

    public Task GoToLogin(string? returnUrl)
        => returnUrl is null
            ? Navigate("//Login")
            : Navigate("//Login", new Dictionary<string, object> { { "ReturnUrl", returnUrl } });

    public Task GoToRegister(string? returnUrl)
        => returnUrl is null
                ? Navigate("//Register")
                : Navigate("//Register", new Dictionary<string, object> { { "ReturnUrl", returnUrl } });

    public Task GoToSelectedOrderDetail(long selectedOrderId)
        => Navigate("OrderDetail", new Dictionary<string, object> { { "OrderId", selectedOrderId } });

    public Task GoToSelectedPieDetail(int selectedPieId)
        => Navigate("PieDetail", new Dictionary<string, object> { { "PieId", selectedPieId } });

    private Task Navigate(string route, IDictionary<string, object>? parameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);

        return parameters != null
            ? Shell.Current.GoToAsync(shellNavigation, parameters)
            : Shell.Current.GoToAsync(shellNavigation);
    }
}