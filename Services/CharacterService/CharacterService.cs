global using AutoMapper;

namespace RPG_game_dotnet.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(dbCharacters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
            var character = await _context.Characters.FirstOrDefaultAsync(c=>c.Id == id);
            if(character is null)
            {
                throw new Exception($"Character with Id '{id}' not found");
            }
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(dbCharacters);
            } 
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.Where(c => c.User!.Id == userId).ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(dbCharacters.ToList());
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c=>c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
            var character = 
                await _context.Characters.FirstOrDefaultAsync(c=>c.Id == updCharacter.Id);
            
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

            await _context.SaveChangesAsync();
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