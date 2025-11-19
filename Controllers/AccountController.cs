using System.Threading.Tasks;
using ClockStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClockStore.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userMgr, SignInManager<ApplicationUser> signInMgr)
        {
            userManager = userMgr;
            signInManager = signInMgr;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel { Name = string.Empty, Password = string.Empty, ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByEmailAsync(loginModel.Name);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel.ReturnUrl ?? "/");
                    }
                }
            }
            ModelState.AddModelError("", "Tên người dùng hoặc mật khẩu không hợp lệ.");
            return View(loginModel);
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = registerModel.Name, Email = registerModel.Email, FullName = registerModel.FullName };
                IdentityResult result = await userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    TempData["message"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            ApplicationUser? user = await userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            ApplicationUser? user = await userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser updatedUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByIdAsync(updatedUser.Id) as ApplicationUser;
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.FullName = updatedUser.FullName;
                user.PhoneNumber = updatedUser.PhoneNumber;

                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["message"] = "Thông tin người dùng đã được cập nhật thành công!";
                    return RedirectToAction("Profile");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(updatedUser);
        }
    }
}