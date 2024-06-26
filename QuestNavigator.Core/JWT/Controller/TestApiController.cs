using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Umbraco.Cms.Web.Common.Security;

namespace QuestNavigator.Core.JWT.Controller;
public class TestApiController : UmbracoApiController
{
	private readonly IConfiguration _config;
	private readonly MemberManager _memberManager;

	public TestApiController(IConfiguration config, MemberManager memberManager)
	{
		_config = config;
		_memberManager = memberManager;
	}

	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public async Task<ActionResult<string>> GetAuthenticatedMessage()
	{
		var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

		var validationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateIssuerSigningKey = true,
			//ValidIssuer = _issuer,
			//ValidAudience = _audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
		};

		var claims = await new JwtSecurityTokenHandler().ValidateTokenAsync(bearerToken, validationParameters);

		return "You need to be authenticated to see this message.";
	}


	[HttpPost]
	public ActionResult<string> GenerateJwtToken([FromBody] UserModel user)
	{
		// verifica user 

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier,user.Username),
			new Claim(ClaimTypes.Role,"apiuser")
		};
		var token = new JwtSecurityToken(_config["Jwt:Issuer"],
			_config["Jwt:Audience"],
			claims,
			expires: DateTime.Now.AddMinutes(50),
			signingCredentials: credentials);


		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public class UserModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}