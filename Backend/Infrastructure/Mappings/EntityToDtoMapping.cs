namespace Infrastructure.Mappings;

public class EntityToDtoMapping : Profile
{
    public EntityToDtoMapping()
    {
        CreateMap<Chore, ChoreDto>().ReverseMap();
        CreateMap<ChoreCategory, ChoreCategoryDto>().ReverseMap();
        
        CreateMap<History, HistoryDto>().ReverseMap();

        CreateMap<Room, RoomDto>().ReverseMap();
        CreateMap<RoomCategory, RoomCategoryDto>().ReverseMap();

        CreateMap<Theme, ThemeDto>().ReverseMap();
        CreateMap<TextTemplate, TextTemplateDto>().ReverseMap();
        CreateMap<TextFragment, TextFragmentDto>().ReverseMap();
    }
}
