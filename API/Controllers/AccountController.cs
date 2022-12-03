using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimPrinciple(HttpContext.User);
                var finalResult = new UserDTO
                {
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user),
                    DisplayName = user.DisplayName
                };
                return new OkObjectResult(new ApiResponse(200, "Ok", finalResult));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            try
            {
                var user = await _userManager.FindByUserByClaimPrincipleWithAddressAsync(HttpContext.User);

                var finalResult = _mapper.Map<Address, AddressDTO>(user.Address);

                return new OkObjectResult(new ApiResponse(200, "Ok", finalResult));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO address)
        {
            try
            {
                var user = await _userManager.FindByUserByClaimPrincipleWithAddressAsync(HttpContext.User);

                user.Address = _mapper.Map<AddressDTO, Address>(address);

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    var finalResult = _mapper.Map<Address, AddressDTO>(user.Address);
                    return new OkObjectResult(new ApiResponse(200, "Ok", finalResult));
                }

                return BadRequest(new ApiResponse(400, "Problem updating the user"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null) return Unauthorized(new ApiResponse(401));

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

                var finalResult = new UserDTO
                {
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user),
                    DisplayName = user.DisplayName
                };
                return new OkObjectResult(new ApiResponse(200, "Ok", finalResult));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            try
            {
                if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
                {
                    return BadRequest(new ApiResponse(403, "This email is already exist."));
                    //return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email address is in use" } });
                }

                var user = new AppUser
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded) return BadRequest(new ApiResponse(400));

                var finalResult = new UserDTO
                {
                    DisplayName = user.DisplayName,
                    Token = _tokenService.CreateToken(user),
                    Email = user.Email
                };
                return new OkObjectResult(new ApiResponse(200, "Ok", finalResult));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            
        }
    }
}
