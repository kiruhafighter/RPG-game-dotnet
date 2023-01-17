using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_game_dotnet.Services.SkillService
{
    public class SkillService : ISkillService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public SkillService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private async Task<bool> SkillExists(int id)
        {
            return await _context.Skills.AnyAsync(s=>s.Id == id);
        }

        public async Task<ServiceResponse<List<GetSkillDto>>> GetAll()
        {
            var response = new ServiceResponse<List<GetSkillDto>>();
            var skills = await _context.Skills.ToListAsync();
            response.Data = _mapper.Map<List<GetSkillDto>>(skills);
            return response;
        }

        public async Task<ServiceResponse<GetSkillDto>> GetById(int id)
        {
            var response = new ServiceResponse<GetSkillDto>();
            if(! await SkillExists(id))
            {
                response.Success = false;
                response.Message = "Skill not found";
                return response;
            }
            var skill = await _context.Skills.FirstOrDefaultAsync(s=>s.Id == id);
            response.Data = _mapper.Map<GetSkillDto>(skill);
            return response;
        }

        public async Task<ServiceResponse<GetSkillDto>> AddSkill(AddSkillDto newSkill)
        {
            var response = new ServiceResponse<GetSkillDto>();
            try
            {
                var skillDb = await _context.Skills.FirstOrDefaultAsync(s=>s.Name.Trim().ToLower() == newSkill.Name.TrimEnd().ToLower());
                if(skillDb != null)
                {
                    throw new Exception("This skill already exists");
                }
                if(newSkill == null)
                {
                    throw new Exception("Invalid value");
                }
                var addSkill = _mapper.Map<Skill>(newSkill);
                _context.Skills.Add(addSkill);
                await _context.SaveChangesAsync();

                var skill = await _context.Skills.FirstOrDefaultAsync(s=>s.Name == newSkill.Name);

                response.Data = _mapper.Map<GetSkillDto>(skill);
                response.Message = "Skill has been added succesfully";
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetSkillDto>> UpdateSkill(int skillId, AddSkillDto updSkill)
        {
            var response = new ServiceResponse<GetSkillDto>();
            try
            {
                if(updSkill == null)
                {
                    throw new Exception("Invalid value");
                }

                if(! await SkillExists(skillId))
                {
                    throw new Exception("Skill not found");
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s=>s.Id == skillId);
                skill!.Name = updSkill.Name;
                skill.Damage = updSkill.Damage;
                skill.Id = skillId;
                _context.Skills.Update(skill);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetSkillDto>(skill);
                response.Message = "Skill has been updated successfully";
            }   
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetSkillDto>>> DeleteSkill (int id)
        {
            var response = new ServiceResponse<List<GetSkillDto>>();
            try
            {
                if(id < 0)
                {
                    throw new Exception("Id value is wrong");
                }

                if(! await SkillExists(id))
                {
                    throw new Exception($"Skill with id {id} not found");
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s=>s.Id == id);
                _context.Skills.Remove(skill!);
                await _context.SaveChangesAsync();

                var skills = await _context.Skills.ToListAsync();
                response.Data = _mapper.Map<List<GetSkillDto>>(skills);
                response.Message = "Skill has been deleted successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

    }
}