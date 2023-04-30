using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SessionFinal.Pages
{
    public class SessionsModel : PageModel
    {
        private readonly UserContext _userContext;

        public SessionsModel(UserContext userContext)
        {
            _userContext = userContext;
        }

        public IList<Session> UserSessions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UserSessions = await _userContext.Sessions
                .Where(us => us.ExpirationTime > DateTime.Now)
                .Include(us => us.Id)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostRevokeAsync(string token)
        {
            var userSession = await _userContext.Sessions
                .FirstOrDefaultAsync(us => us.Token == token);


            if (userSession != null)
            {
                _userContext.Sessions.Remove(userSession);
                await _userContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
