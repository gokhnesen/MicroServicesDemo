using AutoMapper;
using Grpc.Core;
using MicroServicesDemo.Data;
using MicroServicesDemo.Protos;

namespace SyncDataServices.Http.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = _repository.GetAllPlatforms();
        foreach (var platform in platforms)
        {
            response.Platforms.Add(_mapper.Map<GrpcPlatformModel>(platform));
        }
        return Task.FromResult(response);
    }
}
