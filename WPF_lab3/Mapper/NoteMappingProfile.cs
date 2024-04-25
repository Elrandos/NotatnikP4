using AutoMapper;
using WPF_lab3.Dto;
using WPF_lab3.Model;

namespace WPF_lab3.Mapper
{
    public class NoteMappingProfile : Profile
    {
        public NoteMappingProfile()
        {
            CreateMap<Note, NoteDto>().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
        }
    }
}
