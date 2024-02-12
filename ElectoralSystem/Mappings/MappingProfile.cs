using AutoMapper;
using ElectoralSystem.Models.Entities;
using ElectoralSystem.Models.RequestDtos;

namespace ElectoralSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewVoterDto, Voter>()
                .ReverseMap();

            CreateMap<NewCandidateDto, Candidate>()
                .ReverseMap();
            CreateMap<NewPartyDto, Party>()
               .ReverseMap();

        }
    }
}
