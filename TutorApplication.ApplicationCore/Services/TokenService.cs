using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TutorApplication.ApplicationCore.Services.Interfaces;
using TutorApplication.SharedModels.Entities;
using TutorApplication.SharedModels.Models;

namespace TutorApplication.ApplicationCore.Services
{

	public class TokenService:ITokenService
    {
		private readonly JwtOptions _jwtOptions;
		private readonly SymmetricSecurityKey _symmetric;

		public TokenService(IOptions<JwtOptions> jwtOptions)
		{
			_jwtOptions = jwtOptions.Value;
			_symmetric = new(Encoding.UTF8.GetBytes(_jwtOptions.Key));
		}

		public string CreateToken(ApplicationUser user)
		{
			ClaimsIdentity identity = new ClaimsIdentity(new List<Claim>() {

			new Claim(ClaimTypes.Name,user.Email),
			new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
			new Claim(ClaimTypes.Email,user.Email),
			new Claim("roles",user.AccountType),
			});

			SigningCredentials credentials = new(_symmetric, SecurityAlgorithms.HmacSha256Signature);
			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Audience = _jwtOptions.Audience,
				Issuer = _jwtOptions.Issuer,
				SigningCredentials = credentials,
				Subject = identity,
				Expires = DateTime.UtcNow.AddDays(7)
				
			};
			JwtSecurityTokenHandler handler = new();
			var token =  handler.CreateToken(tokenDescriptor);

			return handler.WriteToken(token); ;
		}
	}
}
