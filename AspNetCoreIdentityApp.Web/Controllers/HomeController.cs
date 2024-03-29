﻿using AspNetCoreIdentityApp.Web.Extenisons;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Services;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailServices _emailServices;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailServices emailServices)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Member");

            var hasUser = await _userManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre hatalı.");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new() { "Hesabınız kilitlendi 3 dakika boyunca giriş yapamassınız." });
                return View();
            }

            ModelState.AddModelErrorList(new List<string>()
            {
                $"Email veya Şifre yanlış!",
                $"Başarısız giriş sayısı:{await _userManager.GetAccessFailedCountAsync(hasUser)}"
            });
            return View();
        }


        public async Task<IActionResult> SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
                return View();

            var identityResul = await _userManager.CreateAsync(new()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.Phone
            }, request.PasswordConfirm);


            if (identityResul.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik Kayıt İşlemi Başarıyla Gerçekleşmiştir";
                return RedirectToAction(nameof(HomeController.SignUp));
            }

            ModelState.AddModelErrorList(identityResul.Errors.Select(x => x.Description).ToList());

            //foreach (IdentityError item in identityResul.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, item.Description);
            //}
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPaswordViewModel)
        {
            var hasuser = await _userManager.FindByEmailAsync(resetPaswordViewModel.Email);
            if (hasuser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine ait kullanıcı bulunamadı.");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasuser);
            var passwordResetLink = Url.Action("ReplacePassword", "Home", new
            {
                userId = hasuser.Id,
                token = passwordResetToken
            }, HttpContext.Request.Scheme);

            //https://localhost:7195
            //uhqe npig hnsk nydp

            await _emailServices.SendResetPasswordEmail(passwordResetLink, hasuser.Email);

            TempData["SuccessMessage"] = "Şifre yenileme linki e-posta asdresinize gönderilmiştir.";

            return RedirectToAction(nameof(ResetPassword));
        }


        public IActionResult ReplacePassword(string userId, string token)
        {
            TempData["userId"] = userId; TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReplacePassword(ReplacePasswordViewModel replacePasswordViewModel)
        {
            var userId = TempData["userId"].ToString();
            var token = TempData["token"].ToString();

            var hasUser = await _userManager.FindByIdAsync(userId);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamamıştır");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token, replacePasswordViewModel.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarılı bir şekilde güncellenmiştir";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());

            }

            return View();

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}