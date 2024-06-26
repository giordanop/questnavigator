using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace QuestNavigator.Core.Notifications;

public class HomePageTitleNotificationComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		builder.AddNotificationHandler<ContentPublishingNotification, HomePageTitleNotification>();
	}
}

/// <summary>
/// https://docs.umbraco.com/umbraco-cms/reference/notifications/contentservice-notifications
/// </summary>
public class HomePageTitleNotification : INotificationHandler<ContentPublishingNotification>
{
	public void Handle(ContentPublishingNotification notification)
	{
		foreach (var node in notification.PublishedEntities)
		{
			if (node.ContentType.Alias.Equals("homePage"))
			{
				var title = node.GetValue<string>("title");
				if (title?.Equals("HomePage") == true)
				{
					notification.CancelOperation(new EventMessage("Company policy",
						"Be more creative!", EventMessageType.Error));
				}
			}
		}
	}
}