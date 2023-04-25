using Microsoft.EntityFrameworkCore;

namespace SessionFinal
{
    public class UserContext :DbContext
    {
        public UserContext(DbContextOptions<UserContext> options    ):base(options)
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

}
