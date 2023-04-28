using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SessionFinal.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserContext userContext;

        public LoginModel(UserContext userContext)
        {
            this.userContext = userContext;
        }
        public void OnGet()
        {
        }

      

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            //Establish a database connection
            var user = await userContext.Users
                .SingleOrDefaultAsync(u => u.Email == email && u.HashedPassword == password);

           
                // Check if the user was found
                if (user != null)
                {
                   
                    // Redirect to the home page
                    return RedirectToPage("/Home");
                }
                else
                {
                    // 4. Handle the case where the email or password is incorrect
                    ModelState.AddModelError(string.Empty, "Invalid email or password");
                    return Page();
                }
            }
        

    }
}
