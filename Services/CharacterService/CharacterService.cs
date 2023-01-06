global using AutoMapper;

namespace RPG_game_dotnet.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>()
        {
            new Character(),
            new Character{Id = 1, Name = "Sam"}
        };

        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c=>c.Id) + 1;
            characters.Add(character);
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
            var character = characters.FirstOrDefault(c=>c.Id == id);
            if(character is null)
            {
                throw new Exception($"Character with Id '{id}' not found");
            }
            characters.Remove(character);


            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            } 
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c=>c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
            var character = characters.FirstOrDefault(c=>c.Id == updCharacter.Id);
            if(character is null)
            {
                throw new Exception($"Character with Id '{updCharacter.Id}' not found");
            }

            character.Name = updCharacter.Name;
            character.HitPoints = updCharacter.HitPoints;
            character.Strength = updCharacter.Strength;
            character.Defence = updCharacter.Defence;
            character.Intelligence = updCharacter.Intelligence;
            character.Class = updCharacter.Class;

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            } 
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}