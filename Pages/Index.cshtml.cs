using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SessionFinal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserContext _userContext;
        public IndexModel(ILogger<IndexModel> logger, UserContext userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }
        public bool IsLoggedIn { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            // Check if the session token exists and is valid
            var sessionToken = HttpContext.Request.Cookies["SessionToken"];
            if (!string.IsNullOrEmpty(sessionToken))
            {
                var session = await _userContext.Sessions.FirstOrDefaultAsync(s => s.Token == sessionToken && s.ExpirationTime > DateTime.Now);
                if (session != null)
                {
                    IsLoggedIn = true;
                }
            }

            return Page();
        }
    }
}