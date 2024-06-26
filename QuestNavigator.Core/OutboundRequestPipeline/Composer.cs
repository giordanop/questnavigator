using Umbraco.Cms.Core.Composing;

namespace QuestNavigator.Core.OutboundRequestPipeline;

public class RegisterCustomUrlProviderComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		//builder.UrlProviders().Insert<StationUrlProvider>();
	}
}