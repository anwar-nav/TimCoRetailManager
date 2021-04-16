using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TRMApi.Data;

namespace TRMApi.Controllers
{
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        //This will create a token for the user.
        [Route("/token")]
        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, string grant_type)
        {
            if (await IsValidUsernameandPassword(username, password))
            {
                return new ObjectResult(await GenerateToken(username));
            }
            else
            {
                return BadRequest();
            }
        }

        //For checking the user exist.
        private async Task<bool> IsValidUsernameandPassword(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        //For Generation of Token
        private async Task<dynamic> GenerateToken(string username)
        {
            //This will have complete user details 
            var user = await _userManager.FindByEmailAsync(username);

            //This will have the roles assigned to the user
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            //Creating claims because Json web token uses claim system and these are key value pair.
            //These claims will be used in creating token which will have all these claims for e.g Name
            //of the user, the user id, when claims register, token expiry time and all the role names
            //of user as these are being added in claims in below. Than token is created which is
            //signed that if any value changed in token after decrypting it will invalidate the token.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                //Token should not be valid before (Nbf=NotBefore) certain date and time and which is now.
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                //Token expiration is 1 day after a token is created
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            //Adding the user's role names to the claims created above.
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            //creating a token packet.
            var token = new JwtSecurityToken(
                new JwtHeader(//This will have details of signing
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Secrets:SecurityKey"))),// this is my key
                        SecurityAlgorithms.HmacSha256)),//this is algorithm for signing not encryption.
                new JwtPayload(claims));//This is having the claims which were created above.

            //The same format in which .NetFramework Api returned the token.
            var output = new
            {
                //This actually creates a string of token and Access_Token has this string.
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                //This is username.
                UserName = username //emailaddress of the user.
            };

            return output;

        }
    }
}
