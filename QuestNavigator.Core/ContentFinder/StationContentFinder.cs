using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace QuestNavigator.Core.ContentFinder;

public class StationContentFinder : IContentFinder
{
	private readonly IUmbracoContextAccessor _umbracoContextAccessor;

	public StationContentFinder(IUmbracoContextAccessor umbracoContextAccessor)
	{
		_umbracoContextAccessor = umbracoContextAccessor;
	}

	public Task<bool> TryFindContent(IPublishedRequestBuilder contentRequest)
	{
		var path = contentRequest.Uri.GetAbsolutePathDecoded();
		if (path.StartsWith("/stations-") is false)
		{
			return Task.FromResult(false); // Not found
		}

		if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
		{
			return Task.FromResult(false);
		}

		var content = umbracoContext.Content.GetByRoute(path.Replace("/stations-", "/"));

		if (content is null)
		{
			// If not found, let another IContentFinder in the collection try.
			return Task.FromResult(false);
		}

		// If content is found, then render that node
		contentRequest.SetPublishedContent(content);
		return Task.FromResult(true);
	}
}