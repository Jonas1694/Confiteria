using ArquitecturaModel;
using ArquitecturaModel.Model;
using ArquitecturaModel.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Confiteria.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AplicationDbContext _context;

        public UserController(UserManager<ApplicationUser> userManager, AplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        // GET: UserController
        public ActionResult Index()
        {
            var user = _userManager.Users.Select(s => new GridUsers()
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Apellido = s.Apellido,
                UserName = s.UserName!
            })
                .ToList();
            return View(user);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public async Task<ActionResult> Create()
        {
            await GetSucursales(null);
            await GetRoles("");
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserDto user)
        {
            try
            {
                var newUser = new ApplicationUser
                {
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Direccion = user.Direccion,
                    Telefono = user.Telefono,
                    Email = user.Email,
                    UserName = user.Email,
                    SucursalesId = user.SucursalId,
                    RegistrationDate = DateTime.Now,
                    ModifyByUserId = "Admin",
                    ModifyDescription = "N/A",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (!result.Succeeded)
                {
                    return View();
                }

                if (!string.IsNullOrEmpty(user.Role))
                {
                    var resultRol = await _userManager.AddToRoleAsync(newUser, user.Role);
                    if (!resultRol.Succeeded)
                    {
                        return View();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            var user = _userManager.Users.FirstOrDefault(f => f.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var dto = new UserDto
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Direccion = user.Direccion,
                Telefono = user.Telefono,
                Email = user.Email,
                SucursalId = user.SucursalesId ?? 0,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? "",
            };
            await GetSucursales(dto.SucursalId);
            await GetRoles(dto.Role);
            return View(dto);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, UserDto user)
        {
            try
            {
                var userDto = _userManager.Users.FirstOrDefault(f => f.Id == user.Id);
                if (userDto == null)
                {
                    return NotFound();
                }
                userDto.Nombre = user.Nombre;
                userDto.Apellido = user.Apellido;
                userDto.Direccion = user.Direccion;
                userDto.Telefono = user.Telefono;
                userDto.Email = user.Email;
                userDto.UserName = user.Email;
                userDto.SucursalesId = user.SucursalId;
                var result = await _userManager.UpdateAsync(userDto);
                if (!result.Succeeded)
                {
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            var user = _userManager.Users.FirstOrDefault(f => f.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var dto = new UserDto
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Direccion = user.Direccion,
                Telefono = user.Telefono,
                Email = user.Email,
                SucursalId = user.SucursalesId ?? 0,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? "",
            };
            await GetSucursales(dto.SucursalId);
            await GetRoles(dto.Role);
            return View(dto);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, UserDto user)
        {
            try
            {
                var userDto = _userManager.Users.FirstOrDefault(f => f.Id == id);
                if (userDto == null)
                {
                    return NotFound();
                }
               userDto.IsDelete = true;
                var result = await _userManager.UpdateAsync(userDto);
                if (!result.Succeeded)
                {
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task GetSucursales(int? SucursalId)
        {
            var sucursales = await _context.Sucursales.OrderBy(o => o.SucursalName).ToListAsync();
            if (SucursalId.HasValue)
            {
                ViewData["Sucursales"] = new SelectList(sucursales, "SucursalId", "SucursalName", SucursalId.Value);
            }
            else
            {
                ViewData["Sucursales"] = new SelectList(sucursales, "SucursalId", "SucursalName");
            }
        }
        private async Task GetRoles(string RoleName)
        {
            var roles = await _context.UserRole.OrderBy(o => o.Name).ToListAsync();
            if (!string.IsNullOrEmpty(RoleName))
            {
                ViewData["Roles"] = new SelectList(roles, "Name", "Name", RoleName);
            }
            else
            {
                ViewData["Roles"] = new SelectList(roles, "Name", "Name");
            }
        }
    }
}
