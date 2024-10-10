using BethanysPieShop.Mobile.Data;
using BethanysPieShop.Mobile.Repositories;
using BethanysPieShop.Mobile.Repositories.Interfaces;
using BethanysPieShop.Mobile.Services;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels;
using BethanysPieShop.Mobile.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using static System.Net.WebRequestMethods;

namespace BethanysPieShop.Mobile;

public static class MauiProgram
{
    public static string BethanysPieShopApiClient = "BethanysPieShopApiClient";

    public static string BaseAddress = "http://localhost:5001";

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcon");
            })
            .RegisterServices()
            .RegisterRepositories()
            .RegisterViewModels()
            .RegisterViews();
        
        Routing.RegisterRoute("PieDetail", typeof(PieDetailPage));
        Routing.RegisterRoute("Checkout", typeof(CheckOutPage));
        Routing.RegisterRoute("OrderHistory", typeof(OrderHistoryPage));
        Routing.RegisterRoute("OrderDetail", typeof(OrderDetailPage));

        builder.Logging.AddDebug();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    
    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<ICategoryService, CategoryService>();
        builder.Services.AddTransient<IOrderService, OrderService>();
        builder.Services.AddTransient<IPieService, PieService>();
        builder.Services.AddSingleton<IShoppingCartService, ShoppingCartService>();

        builder.Services.AddTransient<IDeliveryTrackingService, DeliveryTrackingService>();

        return builder;
    }
    
    private static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IAuthRepository, AuthMockRepository>();
        builder.Services.AddTransient<ICategoryRepository, CategoryMockRepository>();
        builder.Services.AddTransient<IOrderRepository, OrderMockRepository>();
        builder.Services.AddTransient<IPieRepository, PieMockRepository>();
        builder.Services.AddTransient<IShoppingCartRepository, ShoppingCartMockRepository>();

        builder.Services.AddHttpClient(BethanysPieShopApiClient,
            client =>
            {
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy())
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        builder.Services.AddTransient<IAuthRepository, AuthRepository>();
        builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
        builder.Services.AddTransient<IOrderRepository, OrderRepository>();
        builder.Services.AddTransient<IPieRepository, PieRepository>();
        builder.Services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>();

        builder.Services.AddSingleton<BethanysPieShopDatabase>();

        return builder;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30), (result, timespan) =>
                {
                    Console.WriteLine("Circuit breaker opened.");
                },
                () =>
                {
                    Console.WriteLine("Circuit breaker closed.");
                });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (outcome, timespan, retryAttempt, context) =>
            {
                Console.WriteLine($"Retrying in {timespan.Seconds} seconds, attempt {retryAttempt}.");
            });
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<CheckoutViewModel>();
        builder.Services.AddSingleton<HomePageViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<PieDetailViewModel>();
        builder.Services.AddSingleton<PiesOverviewViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddSingleton<ShoppingCartViewModel>();
        builder.Services.AddTransient<OrderOverviewViewModel>();
        builder.Services.AddTransient<OrderDetailViewModel>();

        return builder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<CheckOutPage>();
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MyAccountPage>();
        builder.Services.AddTransient<PieDetailPage>();
        builder.Services.AddSingleton<PiesOverviewPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddSingleton<ShoppingCartPage>();
        builder.Services.AddTransient<OrderHistoryPage>();
        builder.Services.AddTransient<OrderDetailPage>();

        return builder;
    }
}