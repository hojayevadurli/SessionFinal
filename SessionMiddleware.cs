using Microsoft.EntityFrameworkCore;

namespace SessionFinal
{
    public class SessionMiddleware : IMiddleware
    {
        private readonly UserContext _userContext;

        public SessionMiddleware(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var sessionToken = context.Session.GetString("SessionToken");
            if (sessionToken != null)
            {
                var session = await _userContext.Sessions
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Token == sessionToken && s.ExpirationTime > DateTime.Now);

                if (session != null)
                {
                    // Update the session's expiration time
                    session.ExpirationTime = DateTime.Now.AddMinutes(30);
                    await _userContext.SaveChangesAsync();

                    // Set the user object on the HttpContext
                    context.Items["User"] = session.User;
                }
            }

            await next(context);
        }
    }

}
