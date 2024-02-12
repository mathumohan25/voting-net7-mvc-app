using Microsoft.AspNetCore.Identity;
using NuGet.Packaging;

namespace ElectoralSystem.Data
{
    public class PasswordHasherService
    {
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public PasswordHasherService(IPasswordHasher<IdentityUser> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(string plainPassword)
        {
            // Replace 'null' with a user instance if you want to simulate hashing a user's password
            return _passwordHasher.HashPassword(null, plainPassword);
        }
    }
    public class UserSeeder
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PasswordHasherService _passwordHasherService;

        public UserSeeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            PasswordHasherService passwordHasherService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasherService = passwordHasherService;
        }



        public async Task CreateRoles()
        {

            string[] roleNames = { "EC_Admin", "Voter", "Anonymous" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public async Task<IList<IdentityUser>> SeedUsers()
        {
            IList<IdentityUser> results = new List<IdentityUser>();
            // Ensure that the user does not already exist
            if (await _userManager.FindByNameAsync("admin@example.com") == null)
            {
                
                var user = new IdentityUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true // Optional: set email confirmation status
                };
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,"Hell0W0rld123$");
                var user1 = new IdentityUser
                {
                    UserName = "mathu@example.com",
                    Email = "mathu@example.com",
                    EmailConfirmed = true // Optional: set email confirmation status
                };
                user1.PasswordHash = _userManager.PasswordHasher.HashPassword(user, "Hell0W0rld123$");
                var user2 = new IdentityUser
                {
                    UserName = "mohan@example.com",
                    Email = "mohan@example.com",
                    EmailConfirmed = true // Optional: set email confirmation status
                };
                user2.PasswordHash = _userManager.PasswordHasher.HashPassword(user, "Hell0W0rld123$");
                // Hash the password before storing
                var hashedPassword = _passwordHasherService.HashPassword("YourInitialAdminPassword");

                // Set the hashed password
                user.PasswordHash = hashedPassword;

                results.Add(user);
                results.Add(user1);
                results.Add(user2);
                await _userManager.CreateAsync(user);
                await _userManager.CreateAsync(user1);
                await _userManager.CreateAsync(user2);
                //if (result.Succeeded)
                {
                    // Optionally, assign roles or additional properties to the user
                    await _userManager.AddToRoleAsync(user, "EC_Admin");
                    await _userManager.AddToRoleAsync(user1, "Voter");
                    await _userManager.AddToRoleAsync(user2, "Voter");
                }
                return results;
            }
            else
            {
                results.AddRange(_userManager.GetUsersInRoleAsync("EC_Admin").Result.ToList());
                results.AddRange(_userManager.GetUsersInRoleAsync("Voter").Result.ToList());
            }
            return results;
        }
    }
}
