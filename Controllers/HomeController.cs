using InAndOut.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InAndOut.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            ViewData["name"] = "Ahmed Maher";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secured()
        {
            return View();
        }
        [Authorize]
        public async Task< IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string username,string password,string returnUrl)
        {

            if (username=="AhmedMaher" && password == "12345")
            {
                List<Claim> Claims = new List<Claim>();
                Claims.Add(new Claim("username", username));
                Claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                Claims.Add(new Claim(ClaimTypes.Name, "Ahmed Maher Ali"));
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(returnUrl);
            }
            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
