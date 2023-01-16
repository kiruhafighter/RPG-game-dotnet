using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPG_game_dotnet.Dtos.Weapon;
using RPG_game_dotnet.Dtos.Skill;

namespace RPG_game_dotnet
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Character, GetCharacterDto>().ReverseMap();
            CreateMap<Character, AddCharacterDto>().ReverseMap();
            CreateMap<Character, UpdateCharacterDto>().ReverseMap();
            CreateMap<Weapon, GetWeaponDto>().ReverseMap();
            CreateMap<Skill, GetSkillDto>().ReverseMap();
            CreateMap<Skill, GetSkillShortDto>().ReverseMap();
            CreateMap<Skill, AddSkillDto>().ReverseMap();
        }
    }
}