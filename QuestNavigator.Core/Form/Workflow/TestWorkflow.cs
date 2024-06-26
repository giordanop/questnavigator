using Microsoft.Extensions.Logging;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Persistence.Dtos;

namespace QuestNavigator.Core.Form.Workflow
{
	public class TestWorkflow : WorkflowType
	{
		private readonly ILogger<TestWorkflow> _logger;

		public TestWorkflow(ILogger<TestWorkflow> logger)
		{
			_logger = logger;

			this.Id = new Guid("ccbeb0d5-adaa-4729-8b4c-4bb439dc0202");
			this.Name = "TestWorkflow";
			this.Description = "This workflow is just for testing";
			this.Icon = "icon-chat-active";
			this.Group = "Services";
		}

		public override Task<WorkflowExecutionStatus> ExecuteAsync(WorkflowExecutionContext context)
		{
			// first we log it
			_logger.LogDebug("the IP " + context.Record.IP + " has submitted a record");

			// we can then iterate through the fields
			foreach (RecordField rf in context.Record.RecordFields.Values)
			{
				// and we can then do something with the collection of values on each field
				List<object> vals = rf.Values;

				// or get it as a string
				rf.ValuesAsString(false);
			}

			//Change the state
			context.Record.State = FormState.Approved;

			_logger.LogDebug("The record with unique id {RecordId} that was submitted via the Form {FormName} with id {FormId} has been changed to {RecordState} state",
				context.Record.UniqueId, context.Form.Name, context.Form.Id, "approved");

			return Task.FromResult(WorkflowExecutionStatus.Completed);
		}

		public override List<Exception> ValidateSettings()
		{
			return new List<Exception>();
		}
	}
}