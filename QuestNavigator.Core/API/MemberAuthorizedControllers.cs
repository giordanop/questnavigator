using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Authorization;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;

namespace QuestNavigator.Core.API
{
	[UmbracoMemberAuthorize("type","group,group1", "member")]
	[Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)] // only works if the controller is routed to /umbraco/backoffice/*. 

	public class AccountController : SurfaceController
	{
		public AccountController(
			IUmbracoContextAccessor umbracoContextAccessor,
			IUmbracoDatabaseFactory databaseFactory,
			ServiceContext services,
			AppCaches appCaches,
			IProfilingLogger profilingLogger,
			IPublishedUrlProvider publishedUrlProvider)
			: base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
		}

		[HttpPost]
		public IActionResult UpdateAccountInfo(string accountInfo)
		{
			// TODO: Update the account info for the current member
			throw new NotImplementedException();
		}
	}
}
