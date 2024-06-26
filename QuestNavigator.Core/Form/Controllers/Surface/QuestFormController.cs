using Microsoft.AspNetCore.Mvc;
using QuestNavigator.Core.Form.Persistence;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Website.Controllers;

namespace QuestNavigator.Core.Form.Controllers.Surface;

public class QuestFormController : SurfaceController
{
	private readonly IScopeProvider _scopeProvider;
	private readonly IMemberManager _memberManager;

	public QuestFormController(
		IUmbracoContextAccessor umbracoContextAccessor,
		IUmbracoDatabaseFactory databaseFactory,
		ServiceContext services,
		AppCaches appCaches,
		IProfilingLogger profilingLogger,
		IPublishedUrlProvider publishedUrlProvider,
		IScopeProvider scopeProvider,
		IMemberManager memberManager)
		: base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
	{
		_scopeProvider = scopeProvider;
		_memberManager = memberManager;
	}

	[HttpPost]
	public async Task<IActionResult> Submit(string hiddenField)
	{
		if (!ModelState.IsValid)
		{
			return CurrentUmbracoPage();
		}

		using var scope = _scopeProvider.CreateScope();

		// Work with form data here
		var model = new QuestAnswer
		{
			AnswerId = Random.Shared.Next(10).ToString(),
			Answer = hiddenField,
			MemberKey = (await _memberManager.GetCurrentMemberAsync()).Key
		};

		scope.Database.Insert(model);
		scope.Complete();

		TempData["Success"] = true;

		return RedirectToCurrentUmbracoPage();
	}

	//   [HttpPost]
	//public IActionResult Submit(QuestFormViewModel model)
	//{
	//	if (!ModelState.IsValid)
	//	{
	//		return CurrentUmbracoPage();
	//	}

	//	// Work with form data here

	//	return RedirectToCurrentUmbracoPage();
	//}
}
