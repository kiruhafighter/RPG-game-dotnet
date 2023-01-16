using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_game_dotnet.Services.SkillService
{
    public interface ISkillService
    {
        Task<ServiceResponse<List<GetSkillDto>>> GetAll();
        Task<ServiceResponse<GetSkillDto>> GetById(int id);
        Task<ServiceResponse<GetSkillDto>> AddSkill(AddSkillDto newSkill);
        Task<ServiceResponse<GetSkillDto>> UpdateSkill(int skillId, AddSkillDto updSkill);
        Task<ServiceResponse<List<GetSkillDto>>> DeleteSkill (int id);
    }
}