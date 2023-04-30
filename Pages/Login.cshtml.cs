using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SessionFinal.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserContext userContext;
        public string Message { get; set; }

        public LoginModel(UserContext userContext)
        {
            this.userContext = userContext;
        }
        public void OnGet(string message)
        {
            Message = message;
        }

      

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            //Establish a database connection
            var user = await userContext.Users
                .SingleOrDefaultAsync(u => u.Email == email);
             
                if (user == null)
                {
                
                    return RedirectToPage("/login", new { message = "Invalid email or password" });
                }
                
            var hashedPassword = PasswordHasher.HashPasword(password, user.Salt);

            if (hashedPassword != user.HashedPassword)
            {
                return RedirectToPage("/login", new { message = "Invalid email or password." });
            }
            var session = userContext.CreateSession(user);
            HttpContext.Session.SetString("SessionToken", session.Token);
            Response.Cookies.Append("SessionToken", session.Token, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(2)
            });

            return RedirectToPage("/Index");

        }
        

    }
}
