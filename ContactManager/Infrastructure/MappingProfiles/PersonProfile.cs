using AutoMapper;
using ContactManager.BLL.DTO;
using ContactManager.DAL.Entity;

namespace ContactManager.Infrastructure.MappingProfiles
{
    public class PersonProfile
    {
        public class UserProfile : Profile
        {
            public UserProfile()
            {
                CreateMap<Person, PersonDto>()
                    .ReverseMap();

            }
        }
    }
}

