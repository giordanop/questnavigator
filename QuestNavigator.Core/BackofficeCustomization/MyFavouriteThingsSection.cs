using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Sections;

namespace QuestNavigator.Core.BackofficeCustomization;

public class MyFavouriteThingsSection : ISection
{
	public string Alias => "myFavouriteThings";

	public string Name => "My Favourite Things";
}

public class SectionComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		builder.Sections().Append<MyFavouriteThingsSection>();
		//builder.Sections().InsertBefore<SettingsSection, MyFavouriteThingsSection>();
		//builder.Sections().InsertAfter<SettingsSection, MyFavouriteThingsSection>();
	}
}
