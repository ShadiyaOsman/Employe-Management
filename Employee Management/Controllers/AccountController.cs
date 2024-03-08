using Employee_Management.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Data.SqlClient;

namespace Employee_Management.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(LoginModel model)
        {
            try
            {
                string connString = "server=.; database=shadia_db; integrated security=true; TrustServerCertificate=True;";
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    string stmt = $"select count(*) total from logintbl where username='{model.Username}' and password='{model.Password}'";
                    SqlCommand cmd = new SqlCommand(stmt, con);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        HttpContext.Session.SetString("username", model.Username);

                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, model.Username),
                            new Claim(ClaimTypes.Role, "Admin")
                        }.ToArray();

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewData["error"] = "Invalid Credentials";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                ViewData["error"] = "An error occurred while processing the request.";
                // You can log the exception or perform any other necessary actions
                return View(model);
            }
        }
    }
}