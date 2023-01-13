using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPG_game_dotnet.Dtos.Weapon;

namespace RPG_game_dotnet.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}