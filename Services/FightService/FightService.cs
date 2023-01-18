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
        private readonly IMapper _mapper;
        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                int[] hitpoints = new int[characters.Count];

                for(int i = 0; i < characters.Count; i++)
                {
                    hitpoints[i] = characters[i].HitPoints;
                }

                bool defeated = false;
                while(!defeated)
                {
                    foreach(var attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if(useWeapon && attacker.Weapon is not null)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else if(!useWeapon && attacker.Skills is not null)
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        else
                        {
                            response.Data.Log
                                .Add($"{attacker.Name} wasn't able to attack!");
                            continue;
                        }

                        response.Data.Log
                            .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage");

                        if(opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log
                                .Add($"{opponent.Name} has been defeated!");
                            response.Data.Log
                                .Add($"{attacker.Name} wins with {attacker.HitPoints} HP left!");
                            break;
                        }
                    }
                }

                characters.ForEach(c=>
                {
                    c.Fights++;
                });

                for(int i = 0; i < characters.Count; i++)
                {
                    characters[i].HitPoints = hitpoints[i];
                }

                await _context.SaveChangesAsync();
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
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                if (attacker is null || opponent is null || attacker.Skills is null)
                {
                    throw new Exception("Something fishi is going on here...");
                }

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);

                if (skill is null)
                {
                    response.Success = false;
                    response.Message = $"{attacker.Name} doesn't know that skill!";
                    return response;
                }

                int damage = DoSkillAttack(attacker, opponent, skill);

                if (damage > 0)
                {
                    response.Message = $"{attacker.Name} did {skill.Name} on {opponent.Name} and dealed {damage} damage";
                }

                if (opponent.HitPoints <= 0)
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
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int pureDamage = skill.Damage + attacker.Intelligence;
            int damage = pureDamage - opponent.Defence;
            if(damage > 0)
            {
                opponent.HitPoints -= damage;
            }
            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                if (attacker is null || opponent is null || attacker.Weapon is null)
                {
                    throw new Exception("Something fishi is going on here...");
                }

                int damage = DoWeaponAttack(attacker, opponent);

                if (damage > 0)
                {
                    response.Message = $"{attacker.Name} attacked {opponent.Name} and dealed {damage} damage";
                }
                if (opponent.HitPoints <= 0)
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
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            if(attacker.Weapon is null)
            {
                throw new Exception("Attacker has no weapon");
            }

            int pureDamage = attacker.Weapon.Damage + attacker.Strength;
            int damage = pureDamage - opponent.Defence;
            if(damage > 0)
            {
                opponent.HitPoints -= damage;
            }
            return damage;
        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
        {
            var characters = await _context.Characters
                .Where(c=>c.Fights>0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c=>c.Defeats)
                .ToListAsync();
            var response = new ServiceResponse<List<HighScoreDto>>()
            {
                Data = characters.Select(c=>_mapper.Map<HighScoreDto>(c)).ToList()
            };
            return response;
        }
    }
}