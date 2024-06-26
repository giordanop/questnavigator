using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace QuestNavigator.Core.Notifications;

public class ContentDeletedAsyncNotification : INotificationAsyncHandler<ContentDeletedNotification>
{
	public async Task HandleAsync(ContentDeletedNotification notification, CancellationToken cancellationToken)
	{
		// await anything
		await Task.Delay(1000);
	}
}

public class NotificationHandlersComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		builder.AddNotificationAsyncHandler<ContentDeletedNotification, ContentDeletedAsyncNotification>();
	}
}