using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Dashboards;

namespace QuestNavigator.Core.BackofficeCustomization.CustomDashboard;

[Weight(-10)]
public class MyCustomDashboard : IDashboard
{
	public string Alias => "myCustomDashboard";

	public string[] Sections => new[]
	{
		Constants.Applications.Content,
		Constants.Applications.Members,
		Constants.Applications.Settings
	};

	public string View => "/App_Plugins/MyCustomDashboard/dashboard.html";

	public IAccessRule[] AccessRules
	{
		get
		{
			var rules = new IAccessRule[]
			{
				new AccessRule {Type = AccessRuleType.Deny, Value = Constants.Security.TranslatorGroupAlias},
				new AccessRule {Type = AccessRuleType.Grant, Value = Constants.Security.AdminGroupAlias}
			};
			return rules;
		}
	}

}