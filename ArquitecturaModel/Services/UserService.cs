using ArquitecturaModel.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaModel.Services
{
    internal class UserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        public UserService(IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userStore = userStore;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users = await _userManager.Users
                .Select(user => new UserDto
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Direccion = user.Direccion,
                Telefono = user.Telefono,
                Email = user.Email,
                SucursalId = user.SucursalId
            }).ToListAsync();
            return users;
        }

        public async Task<bool> CreateUserAsync(UserDto Input)
        {
            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                Nombre = Input.Nombre,
                Apellido = Input.Apellido,
                Direccion = Input.Direccion,
                Telefono = Input.Telefono,
                ModifyByUserId = Input.Email,
                ModifyDescription = Input.Email,
                SucursalId = Input.SucursalId
            };
            await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, Input.Password);
            return result.Succeeded;
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        public async Task<bool> UpdateUserAsync(UserDto Input)
        {
            var user = await _userManager.Users.Where(w=> w.Id == Input.Id).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            user.Nombre = Input.Nombre;
            user.Apellido = Input.Apellido;
            user.Direccion = Input.Direccion;
            user.Telefono = Input.Telefono;
            user.Email = Input.Email;
            user.UserName = Input.Email;
            user.SucursalId = Input.SucursalId;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.Users
                .Where(w => w.Id == userId)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Direccion = user.Direccion,
                    Telefono = user.Telefono,
                    Email = user.Email,
                    SucursalId = user.SucursalId
                }).FirstOrDefaultAsync();
            return user;
        }
    }
}
