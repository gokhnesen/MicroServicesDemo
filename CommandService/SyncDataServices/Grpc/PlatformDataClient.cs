using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using MicroServicesDemo.Protos;

namespace SyncDataServices.Grpc;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PlatformDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public IEnumerable<Platform> ReturnAllPlatforms()
    {
        var address = _configuration["GrpcPlatform"];
        if (string.IsNullOrWhiteSpace(address))
        {
            Console.WriteLine("---> GrpcPlatform address is not configured");
            return Enumerable.Empty<Platform>();
        }

        Console.WriteLine($"--> Calling GRPC Service {address}");

        if (address.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }

        try
        {
            var channel = GrpcChannel.ForAddress(address);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var reply = client.GetAllPlatforms(new GetAllRequest());
            return _mapper.Map<IEnumerable<Platform>>(reply.Platforms);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"---> Could not call GRPC server {ex.Message}");
            return Enumerable.Empty<Platform>();
        }
    }
}
