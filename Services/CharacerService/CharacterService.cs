using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace dotnet_rpg2.Services.CharacerService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var servicResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Characters.Add(character);

            await _context.SaveChangesAsync();

            servicResponse.Data = await _context.Characters
            .Where(c => c.User!.Id == GetUserId())
            .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return servicResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var servicResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {

                var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                if (dbCharacter == null)
                {
                    throw new Exception($"Character with id '{id}' is not found");
                }

                _context.Characters.Remove(dbCharacter);
                await _context.SaveChangesAsync();


                servicResponse.Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                servicResponse.Success = false;
                servicResponse.Message = ex.Message;
            }
            return servicResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var servicResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacter = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .Where(c => c.User!.Id == GetUserId()).ToListAsync();
            servicResponse.Data = dbCharacter.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return servicResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var servicResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            if (dbCharacter == null)
            {
                servicResponse.Success = false;
                servicResponse.Message = $"Character with id '{id}' is not found";
            }
            servicResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);

            return servicResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var servicResponse = new ServiceResponse<GetCharacterDto>();
            try
            {

                var dbCharacter = await _context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if (dbCharacter == null || dbCharacter.User!.Id != GetUserId())
                {
                    throw new Exception($"Character with id '{updatedCharacter.Id}' is not found");
                }

                dbCharacter.Name = updatedCharacter.Name;
                dbCharacter.HitPoints = updatedCharacter.HitPoints;
                dbCharacter.Strength = updatedCharacter.Strength;
                dbCharacter.Defence = updatedCharacter.Defence;
                dbCharacter.Intelligence = updatedCharacter.Intelligence;
                dbCharacter.Class = updatedCharacter.Class;

                // character = _mapper.Map<Character>(updatedCharacter);

                // _mapper.Map(updatedCharacter, character);
                await _context.SaveChangesAsync();


                servicResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            }
            catch (Exception ex)
            {
                servicResponse.Success = false;
                servicResponse.Message = ex.Message;
            }
            return servicResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharaterId && c.User!.Id == GetUserId());
                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found";
                    return response;
                }
                var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill is null)
                {
                    response.Success = false;
                    response.Message = "Skill not found";
                    return response;
                }
                character.Skills!.Add(skill);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
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