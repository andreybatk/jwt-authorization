using AutoMapper;
using JwtAuthorization.API.Contracts;
using JwtAuthorization.BL.Interfaces;
using JwtAuthorization.BL.Responses;
using JwtAuthorization.BL.Validators;
using JwtAuthorization.DB.Entities;
using JwtAuthorization.DB.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtAuthorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthenticatorService _authenticatorService;

        public AuthenticationController(UserManager<User> userManager, RefreshTokenValidator refreshTokenValidator, IMapper mapper, IRefreshTokenRepository refreshTokenRepository, IAuthenticatorService authenticatorService)
        {
            _userManager = userManager;
            _refreshTokenValidator = refreshTokenValidator;
            _mapper = mapper;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticatorService = authenticatorService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new ErrorResponse<string>(errorMessages));
            }

            var registrationUser = _mapper.Map<RegisterRequest, User>(registerRequest);

            IdentityResult result = await _userManager.CreateAsync(registrationUser, registerRequest.Password);
            if (!result.Succeeded)
            {
                IdentityErrorDescriber errorDescriber = new IdentityErrorDescriber();
                IdentityError primaryError = result.Errors.FirstOrDefault();
                var errors = result.Errors;

                return Conflict(new ErrorResponse<IdentityError>(errors));
            }

            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new ErrorResponse<string>(errorMessages));
            }

            User user = await _userManager.FindByNameAsync(loginRequest.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isCorrectPassword)
            {
                return Unauthorized();
            }

            AuthenticatedUserResponse response = await _authenticatorService.AuthenticateAsync(user);

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new ErrorResponse<string>(errorMessages));
            }

            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse<string>("Invalid refresh token."));
            }

            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponse<string>("Invalid refresh token."));
            }

            await _refreshTokenRepository.Delete(refreshTokenDTO.Id);

            User user = await _userManager.FindByIdAsync(refreshTokenDTO.UserId.ToString());
            if (user == null)
            {
                return NotFound(new ErrorResponse<string>("User not found."));
            }

            AuthenticatedUserResponse response = await _authenticatorService.AuthenticateAsync(user);

            return Ok(response);
        }
        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            string rawUserId = HttpContext.User.FindFirstValue("ID");

            if (!Guid.TryParse(rawUserId, out Guid userId))
            {
                return Unauthorized();
            }

            await _refreshTokenRepository.DeleteAll(userId);

            return NoContent();
        }
    }
}