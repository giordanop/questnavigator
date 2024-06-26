using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace QuestNavigator.Core.Search
{
	public class SearchResultPageController : RenderController
	{
		private readonly IPublishedValueFallback _publishedValueFallback;
		private readonly ISearchService _searchService;

		public SearchResultPageController(
			ILogger<RenderController> logger, 
			ICompositeViewEngine compositeViewEngine,
			IUmbracoContextAccessor umbracoContextAccessor, 
			IPublishedValueFallback publishedValueFallback, 
			ISearchService searchService) : 
			base(logger, compositeViewEngine, umbracoContextAccessor)
		{
			_publishedValueFallback = publishedValueFallback;
			_searchService = searchService;
		}

		[HttpGet]
		public override IActionResult Index()
		{
			string queryString = HttpContext.Request.Query["query"];

			// Create the view model and pass it to the view
			SearchViewModel viewModel = new(CurrentPage!, _publishedValueFallback)
			{
				SearchResults = _searchService.SearchContent(queryString),
				HasSearched = !string.IsNullOrEmpty(queryString),
			};

			return CurrentTemplate(viewModel);
		}
	}
}
