namespace Application.Mappings
{
    using Application.Features.Usuario.Command.Editar;
    using AutoMapper;
    using Domain.Entities;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EditarCommand, Usuario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
