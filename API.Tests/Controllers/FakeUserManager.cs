using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace API.Tests.Controllers
{
    public class FakeUserManager : UserManager<AppUser>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<AppUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AppUser>>().Object,
                new IUserValidator<AppUser>[0],
                new IPasswordValidator<AppUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AppUser>>>().Object)
        { }

        //public override Task<IdentityResult> CreateAsync(AppUser user, string password)
        //{
        //    return Task.FromResult(IdentityResult.Success);
        //}

        //public override Task<IdentityResult> AddToRoleAsync(AppUser user, string role)
        //{
        //    return Task.FromResult(IdentityResult.Success);
        //}

        //public override Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
        //{
        //    return Task.FromResult(Guid.NewGuid().ToString());
        //}

        //public override Task<IdentityResult> ConfirmEmailAsync(AppUser user, string token)
        //{
        //    return Task.FromResult(IdentityResult.Success);
        //}

        //public override Task<AppUser> FindByEmailAsync(string email)
        //{
        //    return base.FindByEmailAsync(email);
        //}
    }
}
