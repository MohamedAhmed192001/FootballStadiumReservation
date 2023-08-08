using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using ProjectApi.Dtos;
using System.Threading.Tasks;
using ProjectApi.Configurations;
using ProjectApi.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.PowerBI.Api.Models;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody]UserRegisterationRequestDto dto)
        {
            if(ModelState.IsValid)
            {


                if (string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.LastName) || string.IsNullOrEmpty(dto.Role))
                {
                    return BadRequest(new { error = "The First Name, Last Name or Role can not be Null Or Empty" });
                }

                var SmallRoleCharacters = dto.Role.ToLower();
                if (SmallRoleCharacters != "client" && SmallRoleCharacters != "owner" && SmallRoleCharacters != "admin")
                {
                    return BadRequest(new { error = "The role must be client, owner or admin. Anything else not allowed!" });
                }

                var UserExist = await _userManager.FindByEmailAsync(dto.Email); 
                if (UserExist != null)
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Errors = new List<string>() { "The User Already Exist!" },
                        Result = false

                    }); ;
                }

                var NewUser = new ApplicationUser() 
                { 
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    UserName = dto.Email
                };

                
                var IsCreated = await _userManager.CreateAsync(NewUser, dto.Password);

                if (IsCreated.Succeeded)
                {
                    await _userManager.AddToRoleAsync(NewUser, dto.Role);
                    var Token = GenerateJwtToken(NewUser);
                    return Ok(new AuthenticationResult()
                    {
                        Result = true,
                        Token = Token,
                        Roles = new List<string>() { dto.Role }
                    });
                }
                return BadRequest(new AuthenticationResult()
                {
                    Errors = new List<string>() { "The password must contain 1- At least one uppercase letter " +
                    "2- At least one lowercase letter " +
                    "3- At least one digit At least one non-alphanumeric character " +
                    "4- Minimum length of 6 characters " },
                    Result = false
                    
                }); ;
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto dto)
        {
            if(ModelState.IsValid)
            {
               
                var UserExist = await _userManager.FindByEmailAsync(dto.Email);
                if(UserExist == null) 
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid Email or Password"
                        },

                        Result = false
                    });
                }
                var CorrectUser = await _userManager.CheckPasswordAsync(UserExist, dto.Password);
                if (!CorrectUser)
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid Email or Password"
                        },

                        Result = false
                    });
                }
                var JwtToken = GenerateJwtToken(UserExist);
                return Ok(new AuthenticationResult()
                {
                    Result = true,
                    Token = JwtToken,

                });
            }

            return BadRequest(new AuthenticationResult()
            {
                Errors = new List<string>()
                {
                    "Invalid Email or Password"
                },

                Result = false
            });
        }
        private string GenerateJwtToken(ApplicationUser user)
        { 
            var jwtTokenHandler = new JwtSecurityTokenHandler();    

            var SecretKey = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfig:Secret_Key").Value);

            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id",user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString()),
                }),

                Expires = DateTime.Now.AddDays(7),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecretKey),SecurityAlgorithms.HmacSha256)

            };

            var Token = jwtTokenHandler.CreateToken(TokenDescriptor);
            var JwtToken = jwtTokenHandler.WriteToken(Token);

            return JwtToken;
        }


    }

}
