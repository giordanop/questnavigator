using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.Security;

namespace QuestNavigator.Core.Authentication.Controllers.Render;
public class LoginPageController : RenderController
{
	private readonly IMemberSignInManager _signInManager;
	private readonly IMemberManager _memberManager;

	public LoginPageController(
		ILogger<RenderController> logger,
		ICompositeViewEngine compositeViewEngine,
		IUmbracoContextAccessor umbracoContextAccessor,
		IMemberSignInManager signInManager,
		IMemberManager memberManager)
		: base(logger, compositeViewEngine, umbracoContextAccessor)
	{
		_signInManager = signInManager;
		_memberManager = memberManager;
	}

	[AllowAnonymous]
	[HttpGet]
	public async Task<IActionResult> Index(string? token, string? email)
	{
		if (token.IsNullOrWhiteSpace() || email.IsNullOrWhiteSpace())
			return CurrentTemplate(CurrentPage);

		var memberIdentityUser = await _memberManager.FindByNameAsync(email);
		if (memberIdentityUser == null)
		{
			return Index();
		}

        var result = await _memberManager.VerifyUserTokenAsync(memberIdentityUser, "Default", "passwordless", token);
		
        if (result)
		{
			await _signInManager.SignInAsync(memberIdentityUser, false);
			return Redirect("/");
		}

		ModelState.AddModelError("loginModel", "Member is not authorized");
		return CurrentTemplate(CurrentPage);
	}
}