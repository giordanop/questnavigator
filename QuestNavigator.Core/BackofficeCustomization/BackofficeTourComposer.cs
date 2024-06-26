using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Tour;

namespace QuestNavigator.Core.BackofficeCustomization;
public class BackofficeTourComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		// Filter out all the CMS core tours by alias with a Regex that start with the umbIntro alias
		builder.TourFilters()
			.AddFilter(new BackOfficeTourFilter(pluginName: null, tourFileName: null, tourAlias: new Regex("^umbIntro", RegexOptions.IgnoreCase)));

		//// Filter any tours in the file that is custom-tours.json
		//// Found in App_Plugins/MyCustomBackofficeTour/backoffice/tours/
		//builder.TourFilters()
		//	.AddFilterByFile("custom-tours"); //Without extension

		// Filter out one or more tour JSON files from a specific plugin/package
		// Found in App_Plugins/MyCustomBackofficeTour/backoffice/tours/custom-tours.json
		//builder.TourFilters()
		//	.AddFilterByPlugin("YourTourPlugin");
	}
}
