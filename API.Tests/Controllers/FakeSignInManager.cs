using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace API.Tests.Controllers
{
    public class FakeSignInManager : SignInManager<AppUser>
    {
        private readonly bool _simulateSuccess = false;
        public FakeSignInManager()
            : base(new Mock<FakeUserManager>().Object,
                new HttpContextAccessor(),
                new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<AppUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<AppUser>>().Object)
        { }

        //public override Task<SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure)
        //{
        //    return this.ReturnResult(this._simulateSuccess);
        //}

        //public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        //{
        //    return this.ReturnResult(this._simulateSuccess);
        //}

        //public override Task<SignInResult> CheckPasswordSignInAsync(AppUser user, string password, bool lockoutOnFailure)
        //{
        //    return this.ReturnResult(this._simulateSuccess);
        //}

        //private Task<SignInResult> ReturnResult(bool isSuccess = true)
        //{
        //    SignInResult result = SignInResult.Success;

        //    if (!isSuccess)
        //        result = SignInResult.Failed;

        //    return Task.FromResult(result);
        //}
    }
}
