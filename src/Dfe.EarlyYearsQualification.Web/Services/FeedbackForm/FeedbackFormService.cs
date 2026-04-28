using Dfe.EarlyYearsQualification.Web.Models.Content;
using System.Text;

namespace Dfe.EarlyYearsQualification.Web.Services.FeedbackForm;

public class FeedbackFormService : IFeedbackFormService
{
    public string ConvertQuestionListToString(FeedbackFormPageModel model, string url)
    {
        var sb = new StringBuilder();

        foreach (var question in model.QuestionList)
        {
            // Uses markdown to create a heading
            sb.AppendLine($"## {question.Question}");
            sb.AppendLine(question.Answer);
            if (!string.IsNullOrWhiteSpace(question.AdditionalInfo))
            {
                sb.AppendLine(question.AdditionalInfo);
            }
            // Creates a horizontal rule between questions
            sb.AppendLine();
            sb.AppendLine("---");
        }

        sb.AppendLine($"Submitted from: {url}");

        return sb.ToString();
    }
}