using LocalTourPlanner.Domain;
using Microsoft.AspNetCore.Mvc;
using LocalTourPlanner.Service.Interfaces;

namespace LocalTourPlanner.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private readonly ICustomerService _customerService;
        #endregion

        #region Ctor
        public AccountController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion

        #region Methods

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(Customer customer)
        {
            if (await _customerService.IsUsernameExists(customer.UserName))
            {
                ModelState.AddModelError("", "Username is already taken.");
                return View(customer);
            }

            var result = await _customerService.RegisterCustomer(customer);
            if (result)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Something went wrong. Please try again.");
            return View(customer);
        }

        [HttpGet]
        public IActionResult Login() => View();
  
        [HttpPost]
        public async Task<IActionResult> Login(string UserName, string UserPassword)
        {
            var user = await _customerService.Login(UserName, UserPassword);
            if (user != null)
            {
                // Store common info
                HttpContext.Session.SetInt32("UserID", user.CID ?? 0);
                HttpContext.Session.SetString("UserName", user.CustomerName ?? "User");

                // This grabs the new UserRole from the Domain model and saves it to Session
                HttpContext.Session.SetString("UserRole", user.UserRole ?? "Customer");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Username or Password.");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}