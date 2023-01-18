using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPG_game_dotnet.Dtos.Fight;

namespace RPG_game_dotnet.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request);
        Task<ServiceResponse<List<HighScoreDto>>> GetHighScore();
    }
}