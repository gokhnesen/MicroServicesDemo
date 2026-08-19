using MicroServicesDemo.DTOs;

namespace MicroServicesDemo.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
    }
}
