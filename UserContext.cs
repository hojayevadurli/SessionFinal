using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace SessionFinal
{
    public class UserContext :DbContext
    {
        public UserContext(DbContextOptions<UserContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();
            
        }

        public DbSet<User>Users { get; set; }
        public DbSet<SignupCode> SignupCodes { get; set; }
        public DbSet<Session> Sessions { get; set; }


        public Session CreateSession(User user)
        {
            var session = new Session
            {
               // UserName = user.Email, // Or you could store the user's name instead of email
                Token = Guid.NewGuid().ToString(),
                ExpirationTime = DateTime.Now.AddMinutes(30),
                Id=user.Id,

            };
            Sessions.Add(session);
            SaveChanges();
            return session;
        }
    }


    public class User
    {
        public int Id { get; set; }
    
        public string Email { get; set; }
      
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
    }

    public class SignupCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Email { get; set; }
       
    }

    public class Session
    {
        public int Id { get;set; }       
       
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
        public int IX_Sessions_UserId { get; set; }
        public User User { get; set; }
    }

   

    public static class PasswordHasher
    {
        const int keySize = 64;
        const int iterations = 350000;
        static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        public static string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }
        public static string HashPasword(string password, string salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                Convert.FromHexString(salt),
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }
    }

}
