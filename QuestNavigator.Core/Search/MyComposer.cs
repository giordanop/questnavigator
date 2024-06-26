using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;

namespace QuestNavigator.Core.Search;
public class MyComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		builder.Services.AddTransient<ISearchService, SearchService>();
	}
}


