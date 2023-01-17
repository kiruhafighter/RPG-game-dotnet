using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPG_game_dotnet.Dtos.Fight;

namespace RPG_game_dotnet.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        public FightService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var response = new ServiceResponse<FightResultDto> 
            {
                Data = new FightResultDto()
            };

            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c=>request.CharacterIds.Contains(c.Id))
                    .ToListAsync();
                
                bool defeated = false;
                while(!defeated)
                {
                    foreach(var attacker in characters)
                    {
                        
                    }
                }

            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c=>c.Skills)
                    .FirstOrDefaultAsync(c=>c.Id == request.AttackerId);
                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c=>c.Id == request.OpponentId);
                
                if(attacker is null || opponent is null || attacker.Skills is null)
                {
                    throw new Exception("Something fishi is going on here...");
                }

                var skill = attacker.Skills.FirstOrDefault(s=> s.Id == request.SkillId);

                if(skill is null)
                {
                    response.Success = false;
                    response.Message = $"{attacker.Name} doesn't know that skill!";
                    return response;
                }

                int pureDamage = skill.Damage + attacker.Intelligence;
                int damage = pureDamage - opponent.Defence;
                
                if(damage > 0)
                {
                    opponent.HitPoints -= damage;
                    response.Message = $"{attacker.Name} did {skill.Name} on {opponent.Name} and dealed {damage} damage";
                }

                if(opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated by {attacker.Name}!";
                }

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c=>c.Weapon)
                    .FirstOrDefaultAsync(c=>c.Id == request.AttackerId);
                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c=>c.Id == request.OpponentId);

                if(attacker is null || opponent is null || attacker.Weapon is null)
                {
                    throw new Exception("Something fishi is going on here...");
                }

                int pureDamage = attacker.Weapon.Damage + attacker.Strength;
                int damage = pureDamage - opponent.Defence;

                if(damage > 0)
                {
                    opponent.HitPoints -= damage;
                    response.Message = $"{attacker.Name} attacked {opponent.Name} and dealed {damage} damage";
                }
                if(opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated by {attacker.Name}!";
                }

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}