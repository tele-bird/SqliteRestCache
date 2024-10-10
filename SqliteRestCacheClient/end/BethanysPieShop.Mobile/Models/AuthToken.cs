namespace BethanysPieShop.Mobile.Models;

public class AuthToken
{
    private static readonly TimeSpan Threshold = new(0, 5, 0);

    public string TokenType { get; set; } = default!;

    public string AccessToken { get; set; } = default!;

    public long ExpiresIn { get; set; }

    public string RefreshToken { get; set; } = default!;

    public DateTime ExpirationDateTime { get; set; }

    public bool IsExpired => (ExpirationDateTime - DateTime.UtcNow).TotalSeconds <= Threshold.TotalSeconds;
}