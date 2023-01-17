using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RPG_game_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;
        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse<IEnumerable<GetSkillDto>>))]
        public async Task<IActionResult> GetAllSkills()
        {
            return Ok(await _skillService.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse<GetSkillDto>))]
        public async Task<IActionResult> GetSingle(int id)
        {
            var response = await _skillService.GetById(id);
            if(response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ServiceResponse<GetSkillDto>))]
        public async Task<IActionResult> AddSkill (AddSkillDto newSkill)
        {
            var response = await _skillService.AddSkill(newSkill);
            if(response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(ServiceResponse<GetSkillDto>))]
        public async Task<IActionResult> UpdateSkill (int id, AddSkillDto updSkill)
        {
            var response = await _skillService.UpdateSkill(id, updSkill);
            if(response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse<IEnumerable<GetSkillDto>>))]
        public async Task<IActionResult> DeleteSkill (int id)
        {
            var response = await _skillService.DeleteSkill(id);
            if(response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}