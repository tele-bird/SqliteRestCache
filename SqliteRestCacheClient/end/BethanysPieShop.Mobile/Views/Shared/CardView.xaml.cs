using System.Globalization;

namespace BethanysPieShop.Mobile.Views.Shared;

public partial class CardView
{
    public static readonly BindableProperty CardNameProperty = BindableProperty.Create(
        nameof(CardName), 
        typeof(string), 
        typeof(CardView), 
        string.Empty,
        propertyChanged: OnNamePropertyChanged);

    public static readonly BindableProperty CardImageUrlProperty = BindableProperty.Create(
        nameof(CardImageUrl), 
        typeof(string), 
        typeof(CardView), 
        string.Empty,
        propertyChanged: OnImageUrlPropertyChanged);

    public static readonly BindableProperty CardPriceProperty = BindableProperty.Create(
        nameof(CardPrice), 
        typeof(decimal?), 
        typeof(CardView), 
        null,
        propertyChanged: OnPricePropertyChanged);

    private static void OnNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (CardView)bindable;
        view.Name.Text = newValue as string ?? string.Empty;
    }
    
    private static void OnImageUrlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (CardView)bindable;
        view.ImageUrl.Source = newValue as string;
    }
    
    private static void OnPricePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (CardView)bindable;
        view.Price.Text = ((decimal)newValue).ToString("C", CultureInfo.CurrentCulture);
    }
    
    public string? CardName
    {
        get => GetValue(CardNameProperty) as string;
        init => SetValue(CardNameProperty, value);
    }

    public string? CardImageUrl
    {
        get => GetValue(CardImageUrlProperty) as string;
        init => SetValue(CardImageUrlProperty, value);
    }

    public decimal? CardPrice
    {
        get => (decimal?)GetValue(CardPriceProperty);
        init => SetValue(CardPriceProperty, value);
    }

    public CardView()
    {
        InitializeComponent();
    }
}