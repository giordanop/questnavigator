using global::Umbraco.Cms.Core.HealthChecks;
using global::Umbraco.Cms.Core.Services;
using global::Umbraco.Cms.Infrastructure.HostedServices;

using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.HealthChecks;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.HostedServices;
using Umbraco.Extensions;
using IHostingEnvironment = Umbraco.Cms.Core.Hosting.IHostingEnvironment;


namespace QuestNavigator.Core.BackofficeCustomization;



[HealthCheck("3A482719-3D90-4BC1-B9F8-910CD9CF5B32", "Robots.txt",
	Description = "Create a robots.txt file to block access to system folders.",
	Group = "SEO")]
public class RobotsTxtHealthCheck : HealthCheck
{
	private readonly IHostingEnvironment _hostingEnvironment;
	private readonly ILogger<HealthCheckNotifier> _logger;
	private readonly ILocalizedTextService _textService;

	public RobotsTxtHealthCheck(ILocalizedTextService textService, IHostingEnvironment hostingEnvironment,
		ILogger<HealthCheckNotifier> logger)
	{
		_textService = textService;
		_hostingEnvironment = hostingEnvironment;
		_logger = logger;
	}

	public override Task<IEnumerable<HealthCheckStatus>> GetStatus() =>
		Task.FromResult((IEnumerable<HealthCheckStatus>)new[] { CheckForRobotsTxtFile() });

	public override HealthCheckStatus ExecuteAction(HealthCheckAction action)
	{
		switch (action.Alias)
		{
			case "addDefaultRobotsTxtFile":
				return AddDefaultRobotsTxtFile();
			default:
				throw new InvalidOperationException("Action not supported");
		}
	}

	private HealthCheckStatus CheckForRobotsTxtFile()
	{
		var success = File.Exists(_hostingEnvironment.MapPathContentRoot("~/robots.txt"));
		var message = success
			? _textService.Localize("healthcheck", "seoRobotsCheckSuccess")
			: _textService.Localize("healthcheck", "seoRobotsCheckFailed");

		var actions = new List<HealthCheckAction>();

		if (success == false)
		{
			actions.Add(new HealthCheckAction("addDefaultRobotsTxtFile", Id)
			// Override the "Rectify" button name and describe what this action will do
			{
				Name = _textService.Localize("healthcheck", "seoRobotsRectifyButtonName"),
				Description = _textService.Localize("healthcheck", "seoRobotsRectifyDescription")
			});
		}

		return
			new HealthCheckStatus(message)
			{
				ResultType = success ? StatusResultType.Success : StatusResultType.Error,
				Actions = actions
			};
	}

	private HealthCheckStatus AddDefaultRobotsTxtFile()
	{
		var success = false;
		var message = string.Empty;
		const string content = @"# robots.txt for Umbraco
User-agent: *
Disallow: /umbraco/";

		try
		{
			File.WriteAllText(_hostingEnvironment.MapPathContentRoot("~/robots.txt"), content);
			success = true;
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Could not write robots.txt to the root of the site");
		}

		return
			new HealthCheckStatus(message)
			{
				ResultType = success ? StatusResultType.Success : StatusResultType.Error,
				Actions = new List<HealthCheckAction>()
			};
	}
}