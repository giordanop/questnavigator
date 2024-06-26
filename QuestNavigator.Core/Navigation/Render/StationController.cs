using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using QuestNavigator.Umbraco.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace QuestNavigator.Core.Navigation.Render;

public class StationController : RenderController
{
	private readonly IMemberManager _memberManager;

	public StationController(
		ILogger<RenderController> logger,
		ICompositeViewEngine compositeViewEngine,
		IUmbracoContextAccessor umbracoContextAccessor,
		IMemberManager memberManager)
		: base(logger, compositeViewEngine, umbracoContextAccessor)
	{
		_memberManager = memberManager;
	}

	[HttpGet]
	public new async Task<IActionResult> Index(bool? goTonext = false)
	{
		var siblings = CurrentPage.Parent.Children<Station>();
		var currentStationOrder = siblings.IndexOf(CurrentPage) + 1;

		if (goTonext.Value)
		{

			if (currentStationOrder == siblings.Count())
			{
				throw new Exception("this was the last station");

			}
			else
			{
				var nextStation = siblings.ElementAt(currentStationOrder);
				return Redirect(nextStation.Url());
			}
		}

		if (currentStationOrder == 1)
		{
			return CurrentTemplate(CurrentPage);
		}

		var membershipUser = await _memberManager.GetCurrentMemberAsync();

		var member = _memberManager.AsPublishedMember(membershipUser) as Member;

		var previousStep = CurrentPage.Parent.Children<Station>().ElementAt(currentStationOrder - 1);

		var questionsPreviousStep = previousStep
			.Questions
			.SelectMany(
				q => (q.Content as Question)?.Answers, (_, answer) => answer.Content as Answer);


		//check user replied to questionPreviousStep
		if (true)
		{
			return CurrentTemplate(CurrentPage);
		}
		else
		{
			return Redirect(previousStep.Url());
		}
		return Redirect("");
	}
}