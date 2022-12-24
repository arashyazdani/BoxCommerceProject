using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/account")]
public class AccountController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        ITokenService tokenService, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet("getcurrentuser", Name = "GetCurrentUser")]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
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

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {

        if(string.IsNullOrEmpty(email)) return BadRequest(new ApiResponse(400));

        return await _userManager.FindByEmailAsync(email) != null;

    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDTO>> GetUserAddress()
    {

        var user = await _userManager.FindByUserByClaimPrincipleWithAddressAsync(HttpContext.User);

        var finalResult = _mapper.Map<Address, AddressDTO>(user.Address);

        return new OkObjectResult(new ApiResponse(200, "Ok", finalResult));

    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO address)
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

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        // We don't need to check loginDto because we used data annotation in LoginDTO and it has been checked before.

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

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
    {
        // We don't need to check registerDto because we used data annotation in RegisterDTO and it has been checked before.

        if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            return BadRequest(new ApiResponse(403, "This email is already exist."));
        
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

        HttpContext.Response.Headers.Add("Authorization", string.Format("Bearer {0}", finalResult.Token));

        return new CreatedAtRouteResult("GetCurrentUser",null, new ApiResponse(201, "Created", finalResult));
    }
}