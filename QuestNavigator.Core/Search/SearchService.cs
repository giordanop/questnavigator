using Examine;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;

namespace QuestNavigator.Core.Search
{
	public interface ISearchService
	{
		IEnumerable<IPublishedContent> SearchContent(string query);
	}

	public class SearchService : ISearchService
	{
		private readonly IExamineManager _examineManager;
		private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;

		public SearchService(IExamineManager examineManager, IUmbracoHelperAccessor umbracoHelperAccessor)
		{
			_examineManager = examineManager;
			_umbracoHelperAccessor = umbracoHelperAccessor;
		}

		public IEnumerable<IPublishedContent> SearchContent(string query)
		{
			// get umbracoHelper from the accessor
			_umbracoHelperAccessor.TryGetUmbracoHelper(out var umbracoHelper);

			IEnumerable<string> ids = Array.Empty<string>();
			if (!string.IsNullOrEmpty(query) && _examineManager.TryGetIndex("ExternalIndex", out IIndex? index))
			{
				ids = index
					.Searcher
					.CreateQuery("content")
					//.NodeTypeAlias("person")
					//.And()
					.Field("nodeName", query)
					.Execute()
					.Select(x => x.Id);
			}

			foreach (var id in ids)
			{
				yield return umbracoHelper.Content(id);
			}
		}
	}
}
