using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace QuestNavigator.Core.Form.Persistence;

[TableName("QuestAnswers")]
[PrimaryKey("Id", AutoIncrement = true)]
[ExplicitColumns]
public class QuestAnswer
{
    [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("AnswerId")]
    public string? AnswerId { get; set; }
    
    [Column("MemberKey")]
    public Guid? MemberKey { get; set; }

	[Column("Answer")]
    public string? Answer { get; set; }
}