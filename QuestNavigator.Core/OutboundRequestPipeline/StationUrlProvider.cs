using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace QuestNavigator.Core.OutboundRequestPipeline;

public class StationUrlProvider : DefaultUrlProvider
{
	public StationUrlProvider(
		IOptionsMonitor<RequestHandlerSettings> requestSettings,
		ILogger<DefaultUrlProvider> logger,
		ISiteDomainMapper siteDomainMapper,
		IUmbracoContextAccessor umbracoContextAccessor,
		UriUtility uriUtility,
		ILocalizationService localizationService)
		: base(requestSettings, logger, siteDomainMapper, umbracoContextAccessor, uriUtility, localizationService)
	{
	}

	public override UrlInfo? GetUrl(IPublishedContent content, UrlMode mode, string? culture, Uri current)
	{
		if (content is null)
		{
			return null;
		}

		// Only apply this to product pages
		if (content.ContentType.Alias == "station")
		{
			// Get the original base url that the DefaultUrlProvider would have returned,
			// it's important to call this via the base, rather than .Url, or UrlProvider.GetUrl to avoid cyclically calling this same provider in an infinite loop!!)
			UrlInfo? defaultUrlInfo = base.GetUrl(content, mode, culture, current);
			if (defaultUrlInfo is null)
			{
				return null;
			}

			if (!defaultUrlInfo.IsUrl)
			{
				// This is a message (eg published but not visible because the parent is unpublished or similar)
				return defaultUrlInfo;
			}
			else
			{
				// Manipulate the url somehow in a custom fashion:
				var originalUrl = defaultUrlInfo.Text.TrimStart('/');
				var customUrl = $"stations-{originalUrl}";
				return new UrlInfo(customUrl, true, defaultUrlInfo.Culture);
			}
		}
		// Otherwise return null
		return null;
	}
}