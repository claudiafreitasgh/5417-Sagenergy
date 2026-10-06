using _5417_Sagenergy.Data;
using _5417_Sagenergy.Data.Entities;
using _5417_Sagenergy.Helpers;
using _5417_Sagenergy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace _5417_Sagenergy.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public AccountController(
                IUserHelper userHelper,
                DataContext context)
        {
            _userHelper = userHelper;
            _context = context;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);

                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to login!");

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            "The user couldn't be created.");

                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, "Customer");

                    var client = new Client
                    {
                        Name = $"{model.FirstName} {model.LastName}",
                        Email = model.Username,
                        UserId = user.Id
                    };

                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();

                    var loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.Username
                    };

                    var result2 = await _userHelper.LoginAsync(loginViewModel);

                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(
                        string.Empty,
                        "The user couldn't be logged.");
                }
            }

            return View(model);
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);

                if (user != null)
                {
                    var token = await _userHelper.GeneratePasswordResetTokenAsync(user);

                    return RedirectToAction(
                        nameof(ResetPassword),
                        new
                        {
                            email = model.Email,
                            token = token
                        });
                }

                ModelState.AddModelError(
                    string.Empty,
                    "No user was found with this email.");
            }

            return View(model);
        }

        public IActionResult ResetPassword(
            string email,
            string token)
        {
            if (string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(token))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userHelper.ResetPasswordAsync(
                        user,
                        model.Token,
                        model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Login));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "User not found.");
                }
            }

            return View(model);
        }


        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    var response = await _userHelper.UpdateUserAsync(user);

                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(
                    this.User.Identity.Name);

                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(
                        user,
                        model.OldPassword,
                        model.NewPassword);

                    if (result.Succeeded)
                    {
                        ViewBag.UserMessage = "Password changed successfully!";
                        return View();
                    }

                    ModelState.AddModelError(
                        string.Empty,
                        result.Errors.FirstOrDefault().Description);
                }
                else
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "User not found.");
                }
            }

            return View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }


    }
}
