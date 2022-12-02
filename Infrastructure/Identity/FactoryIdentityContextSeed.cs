using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity
{
    public class FactoryIdentityContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!userManager.Users.Any())
                {
                    var user = new AppUser
                    {
                        DisplayName = "Arash",
                        Email = "arash.yazdani.b@gmail.com",
                        UserName = "arash.yazdani.b@gmail.com",
                        PhoneNumber = "+37495234622",
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                        Address = new Address
                        {
                            FirstName = "Arash",
                            LastName = "Yazdani",
                            Street = "10 The Street",
                            City = "New York",
                            State = "NY",
                            Zipcode = "90210"
                        }
                    };

                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<FactoryIdentityContext>();
                logger.LogError(ex.Message);
            }
            
        }
    }
}
