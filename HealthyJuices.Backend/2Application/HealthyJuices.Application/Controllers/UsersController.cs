using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Application.Controllers
{
    public class UsersController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.Query()
                .ToListAsync();

            var result = users.Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<List<UserDto>> GetAllActiveAsync()
        {
            var users = await _userRepository.Query()
                .IsActive()
                .IsNotRemoved()
                .ToListAsync();

            var result = users.Select(x => x.ToDto())
                .ToList();

            return result;
        }

        public async Task<List<UserDto>> GetAllActiveByUserRoleAsync(UserRole role)
        {
            var users = await _userRepository.Query()
                .IsActive()
                .ByUserRole(role)
                .ToListAsync();

            var result = users.Select(x => x.ToDto())
                .ToList();

            return result;
        }


        public async Task<UserDto> GetAsync(long id)
        {
            var user = await _userRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            var result = user.ToDto();

            return result;
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await _userRepository.Query()
                .ByEmail(email)
                .SingleOrDefaultAsync();

            var result = user?.ToDto();

            return result;
        }

        public async Task<long> CreateAsync(AddOrEditUserDto dto)
        {
            if (await _userRepository.IsExistingAsync(dto.Email))
                throw new InauspiciousException($"User '{dto.Email}' already existing");

            var user = new User(dto.Email, dto.Password, dto.Roles);
            await _userRepository.Insert(user).SaveChangesAsync();
            return user.Id;
        }

        public async Task DeleteAsync(long id)
        {
            var user = await _userRepository.Query()
                .ById(id)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new InauspiciousException($"User not found");

            user.Remove();

            await _userRepository.Update(user).SaveChangesAsync();
        }
    }
}