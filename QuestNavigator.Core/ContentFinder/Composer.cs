using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Routing;

namespace QuestNavigator.Core.ContentFinder;

public class UpdateContentFindersComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		// Add our custom content finder just before the core ContentFinderByUrl
		builder.ContentFinders().InsertBefore<ContentFinderByUrl, StationContentFinder>();
	}
}