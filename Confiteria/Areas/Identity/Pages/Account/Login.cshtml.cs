// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using ArquitecturaModel.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Confiteria.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AplicationRole> _roleManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager, RoleManager<AplicationRole> roleManager)
        {
            _signInManager = signInManager;
			_userManager = userManager;	
			_roleManager = roleManager;
			_logger = logger;
		}

        [BindProperty]
        public InputModel Input { get; set; }
		
		public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
        
        [TempData]
        public string ErrorMessage { get; set; }
        public class InputModel
        {
            [Display(Name = "Usuario")]
            [Required(ErrorMessage = "El Campo de Usuario es Obligatorio")]
            [EmailAddress(ErrorMessage = "Los Datos ingresados no son validos")]
            public string Email { get; set; }

            [Display(Name = "Contraseña")]
            [Required(ErrorMessage = "El Campo Contraseña Es Obligatorio")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
			returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
			
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
			await CreateRoles();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

		private async Task CreateRoles()
		{
			IdentityResult roleResult;
			//string email = "veroviera1302@gmail.com";

			//Check that there is an Administrator role and create if not
			//Task<bool> hasAdminRole = RoleManager.RoleExistsAsync("Admin");
			//IdentityResult roleResult;
			string[] roleNames = { "Admin", "Facturador","SuperUsuario" };
			foreach (var roleName in roleNames)
			{
				var roleExist = await _roleManager.RoleExistsAsync(roleName);
				if (!roleExist)
				{
					//create the roles and seed them to the database: Question 1
					roleResult = await _roleManager.CreateAsync(new AplicationRole() { Name = roleName, DescripcionRole = roleName, RegistrationDate = DateTime.Now });
				}
			}
			//Check if the admin user exists and create it if not
			//Add to the Administrator role

			//var testUser = await _userManager.FindByEmailAsync(email);
			//if (testUser == null)
			//{
			//	ApplicationUser administrator = new ApplicationUser();
			//	administrator.Email = email;
			//	administrator.UserName = email;
			//	administrator.Nombre = "Administrador";
			//	administrator.Apellido = "del Sistema";
			//	administrator.PhoneNumber = "000000000";
			//	administrator.Direccion = "N/A";
			//	administrator.Telefono = "N/A";
			//	var newUser = await _userManager.CreateAsync(administrator, "Admin2023.");

			//	if (newUser.Succeeded)
			//	{
			//		var newUserRole = await _userManager.AddToRoleAsync(administrator, "Admin");

			//		if (newUserRole.Succeeded)
			//		{
			//			var code = await _userManager.GenerateEmailConfirmationTokenAsync(administrator);
			//			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			//			var callbackUrl = Url.Page(
			//				"/Account/ConfirmEmail",
			//				pageHandler: null,
			//				values: new { area = "Identity", userId = administrator.Id.ToString(), code = code },
			//				protocol: Request.Scheme);

			//			//mailHelper.SendMail(administrator.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
			//		}
			//	}
			//}
			//email = "tzskill01@gmail.com";
			//testUser = await _userManager.FindByEmailAsync(email);
			//if (testUser == null)
			//{
			//	ApplicationUser administrator = new ApplicationUser();
			//	administrator.Email = email;
			//	administrator.UserName = email;
			//	administrator.Nombre = "Facturador";
			//	administrator.Apellido = "del Sistema";
			//	administrator.PhoneNumber = "000000000";
			//	administrator.Direccion = "N/A";
			//	administrator.Telefono = "N/A";
			//	Task<IdentityResult> newUser = _userManager.CreateAsync(administrator, "Facturador2023.");
			//	newUser.Wait();

			//	if (newUser.Result.Succeeded)
			//	{
			//		Task<IdentityResult> newUserRole = _userManager.AddToRoleAsync(administrator, "Facturador");
			//		newUserRole.Wait();
			//		var code = await _userManager.GenerateEmailConfirmationTokenAsync(administrator);
			//		code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			//		var callbackUrl = Url.Page(
			//			"/Account/ConfirmEmail",
			//			pageHandler: null,
			//			values: new { area = "Identity", userId = administrator.Id.ToString(), code = code },
			//			protocol: Request.Scheme);

			//		//mailHelper.SendMail(administrator.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
			//	}
			//}

			//email = "veroviera1302@gmail.com";


			//string[] TipoDocumento = { "V", "E", "J", "G" };
			//foreach (var item in TipoDocumento)
			//{
			//	if (!Context.TipoDocumento.Any(a => a.Documento == item))
			//	{
			//		var tipoDocumento = new TipoDocumento { TipoDocumentoId = Guid.NewGuid().ToString(), Documento = item };
			//		Context.TipoDocumento.Add(tipoDocumento);
			//		await Context.SaveChangesAsync();
			//	}
			//}

		}
	}
}
