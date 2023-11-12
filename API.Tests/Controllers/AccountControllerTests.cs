using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using API.Controllers;
using API.DTOs;
using API.Errors;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace API.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly AccountController _accountController;
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<IMapper> _iMapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ApiResponse> _apiResponse;
        private readonly HttpClient httpClient;

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
            _apiResponse = new Mock<ApiResponse>();
            httpClient = new HttpClient();
            _iMapper = new Mock<IMapper>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _tokenService = new Mock<ITokenService>();
            
            var fakeUserManager = new Mock<FakeUserManager>();
            var fakeSignInManager = new Mock<FakeSignInManager>();

            //var fakeUserManager1 = GetMockUserManager();
            //var fakeSignInManager1 = GetMockSignInManager();

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

        [Fact]
        public async Task AddTermWithAuthorization()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "api/account/login");

            request.Content = new StringContent(JsonSerializer.Serialize(new
            {
                //term = "MFA",
                //definition = "An authentication process that considers multiple factors."
                email= "arash",
                password= "Pa$$"
            }), Encoding.UTF8, "application/json");

            var accessToken = FakeJwtManager.GenerateJwtToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await httpClient.SendAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(false, response.IsSuccessStatusCode);
            
            //var responseString = response.Content.ReadAsStringAsync().Result;

            //dynamic jsonObject = JObject.Parse(responseString);
            //int statusCode = (int)jsonObject.StatusCode;
            //Assert.Equal(400, statusCode);


            //Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //var resposeConvert = response.Content.ReadAsStringAsync();
            //var aa = resposeConvert.Result.FirstOrDefault();
            // if your action was returning data in the body like: Content<string>(HttpStatusCode.Accepted, "some updated data");

            //ApiException negResult = Assert.IsType<ApiException>(response);
            //Assert.Equal(HttpStatusCode.Accepted, negResult.StatusCode);
            //var aa = response.RequestMessage;
            //Assert.Equal(aa.ToString(), negResult.StatusCode.ToString());

            //ApiResponse aa = Assert.IsType<ApiResponse>(response.Content);
            ////Assert.Equal(HttpStatusCode.BadRequest.ToString(), response.StatusCode.ToString());

            //Assert.Single(((ApiResponse)response.Content).StatusCode);
            //Assert.Equal("/api/account/login", response.Content.Headers.GetValues("statusCode").FirstOrDefault());
        }

        //private Mock<UserManager<AppUser>> GetMockUserManager()
        //{
        //    return new Mock<UserManager<AppUser>>(
        //        new Mock<IUserStore<AppUser>>(), 
        //        new Mock<IOptions<IdentityOptions>>().Object, 
        //        new Mock<IPasswordHasher<AppUser>>().Object,
        //        new IUserValidator<AppUser>[0],
        //        new IPasswordValidator<AppUser>[0],
        //        new Mock<ILookupNormalizer>().Object,
        //        new Mock<IdentityErrorDescriber>().Object,
        //        new Mock<IServiceProvider>().Object,
        //        new Mock<ILogger<UserManager<AppUser>>>().Object);
        //}

        //private Mock<SignInManager<AppUser>> GetMockSignInManager()
        //{
        //    return new Mock<SignInManager<AppUser>>(
        //        new Mock<FakeUserManager>().Object,
        //        new HttpContextAccessor(),
        //        new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
        //        new Mock<IOptions<IdentityOptions>>().Object,
        //        new Mock<ILogger<SignInManager<AppUser>>>().Object,
        //        new Mock<IAuthenticationSchemeProvider>().Object,
        //        new Mock<IUserConfirmation<AppUser>>().Object);
        //}
    }
}
