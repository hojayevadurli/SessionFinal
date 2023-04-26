using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SessionFinal.Pages
{
    public class CreateAccountModel : PageModel
    {
        private readonly UserContext userContext;

        public string Code { get; set; }
        public string Email { get; private set; }

        public CreateAccountModel(string code, UserContext userContext )
        {
            Code = code;
            this.userContext = userContext;
           
        }

       
        public async Task<IActionResult> OnGet(string code)
        {
            var codeRow=await userContext.SignupCodes.FirstOrDefaultAsync(x => x.Code == code);
            if (codeRow == null)
            {
                return RedirectToPage("/signup", new { message = "Invalid code" });
                
            }
            else if(codeRow.ExpiresOn<DateTime.Now){
                return RedirectToPage("/signup", new { message = "Expired Code" });
            }
            else
            {
                Code = code;
                Email = codeRow.Email;
                return Page();
            }
        }

        public async Task<IActionResult> OnPost(string password, string code)
        {
            var hashedPassword=PasswordHasher.HashPasword(password,out var salt);
            var accountCode= await userContext.SignupCodes.FirstOrDefaultAsync(c=>c.Code==Code);
            if (accountCode == null)
            {
                return RedirectToPage("/signup", new { message = "Invalid code" });
            }
            else if (accountCode.ExpiresOn < DateTime.Now)
            {
                return RedirectToPage("/signup", new { message = "Expired Code" });
            }

            var userAccount = new User
            {
                Email = accountCode.Email,
                HashedPassword = hashedPassword,
                Id = accountCode.Id
            };
            userContext.Users.Add(userAccount);
            await userContext.SaveChangesAsync();

            return Page();
        }


    }
}
