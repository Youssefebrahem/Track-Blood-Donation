using Blood_Donation.Models;
using Blood_Donation.Repository;
using Blood_Donation.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blood_Donation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepo _accountRepo;

        public AccountController(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepo.GetUserByUserNameAsync(model); // Await the asynchronous method

                if (user != null && user.Password == model.Password)
                {
                    // Create claims for the logged-in user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Ensure this for retrieving AccountId
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal); // Specify the scheme

                    return RedirectToAction("Index", "Blood");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password."); // Use empty key for general error
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Specify the scheme
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Account acc)
        {
            if (ModelState.IsValid)
            {
                await _accountRepo.RegisterAsync(acc); // Await the asynchronous method
                return RedirectToAction("Login", "Account");
            }
            return View(acc);
        }
    }
}
