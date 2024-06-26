using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestNavigator.Core.Form.Models;
public class QuestFormViewModel
{
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public List<QuestionAnswerViewModel> QAs { get; set; }
}

public class QuestionAnswerViewModel
{
    public QuestionViewModel Question { get; set; }
    public AnswerViewModel SelectedAnswer { get; set; }
    public List<QuestionAnswerViewModel> ChildQuestions { get; set; }
}
public class QuestionViewModel
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public List<AnswerViewModel> PossibleAnswers { get; set; }
    public string SelctedAnswer { get; set; }
    public int QuestionNumber { get; set; }
}
public class AnswerViewModel
{
    public Guid Id { get; set; }
    public string AnswerText { get; set; }
}