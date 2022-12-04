using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using API.DTOs;
using API.Errors;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace API.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly AccountController _accountController;
        //private readonly Mock<UserManager<AppUser>> _userManager;
        //private readonly Mock<AppUser> _appUser;
        //private readonly Mock<SignInManager<AppUser>> _signInManager;
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<IMapper> _iMapper;
        private readonly Mock<ApiResponse> _apiResponse;

        public AccountControllerTests()
        {
            var users = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.it"
                }

            }.AsQueryable();

            _iMapper = new Mock<IMapper>();
            _tokenService = new Mock<ITokenService>();
            var fakeUserManager = new Mock<FakeUserManager>();
            var fakeSignInManager = new Mock<FakeSignInManager>();

            fakeUserManager.Setup(x => x.Users)
                .Returns(users);
            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);

            fakeSignInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            _accountController = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, _tokenService.Object,_iMapper.Object);
        }

        [Fact]
        public async Task AccountLogin_GetBadRequest()
        {
            var loginInputData = Mock.Of<LoginDTO>(x => x.Email == "arash.yazdani.bgmail.com" && x.Password == "Pa$$w0rd");

            var result = (await _accountController.Login(loginInputData) as ActionResult<UserDTO>);

            Assert.NotNull(result);

            Assert.IsType<ActionResult<UserDTO>>(result);
        }
 
    }
}
