using TrackingServiceReference;

namespace BethanysPieShop.Mobile.Services.Interfaces;

public interface IDeliveryTrackingService
{
    Task<TrackingInformation[]> GetTrackingInformation(long trackingCode);
}
