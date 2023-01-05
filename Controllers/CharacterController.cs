
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace RPG_game_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private static List<Character> characters = new List<Character>()
        {
            new Character(),
            new Character{Id = 1, Name = "Sam"}
        };

        [HttpGet("GetAll")]
        // [ProducesResponseType(200,Type = typeof(Character))]
        public ActionResult<List<Character>> Get()
        {
            return Ok(characters);
        }

        [HttpGet("{id}")]
        // [Route("FirstHero")]
        public ActionResult<Character> GetSingle(int id)
        {
            return Ok(characters.FirstOrDefault(c=>c.Id == id));
        }

        [HttpPost]
        public ActionResult<List<Character>> AddCharacter ([FromBody] Character newChar)
        {
            characters.Add(newChar);
            return Ok(characters);
        }
        
    }
}