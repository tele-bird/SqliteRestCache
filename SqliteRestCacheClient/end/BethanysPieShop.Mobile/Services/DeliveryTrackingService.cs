using BethanysPieShop.Mobile.Services.Interfaces;
using TrackingServiceReference;

namespace BethanysPieShop.Mobile.Services;

public class DeliveryTrackingService : IDeliveryTrackingService
{
    public async Task<TrackingInformation[]> GetTrackingInformation(long trackingCode)
    {
        try
        {
            TrackingSoapClient client = new(TrackingSoapClient.EndpointConfiguration.TrackingSoap);
            return await client.TrackingInformationAsync(trackingCode);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}
