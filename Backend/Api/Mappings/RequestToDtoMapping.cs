namespace Api.Mappings;

public class RequestToDtoMapping : Profile
{
    public RequestToDtoMapping()
    {
        CreateMap<LoginRequest, LoginDto>().ReverseMap();
        CreateMap<RegisterRequest, RegisterDto>().ReverseMap();
    }
}
