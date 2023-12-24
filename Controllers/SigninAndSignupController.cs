using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using System.Security.Claims;

namespace OnlineStore.Controllers
{
    public class SigninAndSignupController : Controller
    {

        private readonly ModelContext _context;

        //Decalre verible from IWebHostEnvironment
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SigninAndSignupController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;  //Dependency Injection 
        }

        public IActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn([Bind("Email,Password")] Logininfo loginInfo, string returnUrl)
        {
            var auth = _context.Logininfos
                       .Where(x => x.Email == loginInfo.Email && x.Password == loginInfo.Password)
                       .SingleOrDefault();

            if (auth != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, auth.Email),
            new Claim(ClaimTypes.Role, auth.Userroleid.ToString()),
            // Add other claims as needed
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Or set based on a "Remember Me" input from the sign-in form
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                // Set any session variables you need
                HttpContext.Session.SetInt32("UserId", (int)auth.Userid);

                // Redirect to returnUrl if it exists and is valid
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Response.Cookies.Delete("returnUrl");
                    return Redirect(returnUrl);
                }
                // You should also delete the cookie if the URL is not valid or empty
                Response.Cookies.Delete("returnUrl");


                // Otherwise, proceed with the role-based redirection
                switch (auth.Userroleid)
                {
                    case 1:
                        HttpContext.Session.SetString("AdminName", auth.Email);
                        return RedirectToAction("Dashboard", "Admin");
                    case 2:
                        // We've already set the session for UserId above
                        return RedirectToAction("Index", "Home");
                    default:
                        // Handle other roles or default case
                        // You might want to redirect to a default page or show an error
                        break;
                }
            }

            else
            {
                // If authentication failed, add an error to the ModelState
                ModelState.AddModelError("LoginError", "The email address or password you entered is incorrect.");
            }
            return View(loginInfo);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("Userid,Fname,Lname,Birthdate,Gender,Usercity,Phonenumber,ImageFile")] User user,
    string Email, string password)
        {
            if (ModelState.IsValid)
            {
                // same of create code 
                if (user.ImageFile != null)
                {
                    string wwwRoutPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string path = Path.Combine(wwwRoutPath + "/images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(fileStream);
                    }
                    user.Imagepath = fileName;
                }
                _context.Add(user);
                await _context.SaveChangesAsync();
                //add user login details 

               Logininfo login = new Logininfo();
                login.Userroleid = 2;
                login.Email = Email;
                login.Password = password;
                login.Userid = user.Userid; 
                _context.Add(login);
                await _context.SaveChangesAsync();

                // after user reagister thay go to login page 
                return RedirectToAction("SignIn");
            }
            // this if my code dosn`t work will execute it 
            //if the user dosn`t registed  , the user still in register
            return View("SignUp");
        }



        public async Task<IActionResult> SignOut()
        {
            HttpContext.Session.Clear();
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

    }
}
