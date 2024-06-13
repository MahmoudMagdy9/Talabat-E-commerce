using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTO;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.API.Controllers
{

    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuthService authService,
            IMapper mapper)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }
        //login
        [HttpPost("login")] //api/account/login
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDTO()
            {
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager),
                DisplayName = user.DisplayName
            });

        }
        //add register
        [HttpPost("register")] //api/account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (CheckEmailExist(registerDto.Email).Result.Value)
                return BadRequest();



            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split("@")[0],
                PhoneNumber = registerDto.Phone
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = await _authService.CreateTokenAsync(user, _userManager),
                Email = user.Email
            };
        }

        [Authorize]
        [HttpGet] //api/account/getcurrentuser
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return new UserDTO
            {
                Email = email,
                DisplayName = user?.DisplayName ?? string.Empty,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            };

        }
        [Authorize]
        [HttpGet("address")] //api/account/address
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            return Ok(_mapper.Map<AddressDTO>(user.Address));
        }

        [Authorize]
        [HttpPut("address")] //api/account/address
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO address)
        {
            var updatedAddress = _mapper.Map<Address>(address);
            var user = await _userManager.FindUserWithAddressAsync(User);
            updatedAddress.Id = user.Address.Id;
            user.Address = updatedAddress;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<AddressDTO>(user.Address));
            return BadRequest("Problem updating the user");
        }

        [HttpGet("emailexist")] //api/account/emailexist
        public async Task<ActionResult<bool>> CheckEmailExist(string email) 
            => await _userManager.FindByEmailAsync(email) != null;

    }
}
