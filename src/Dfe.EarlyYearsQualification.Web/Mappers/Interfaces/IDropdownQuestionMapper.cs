using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IDropdownQuestionMapper
{
    Task<DropdownQuestionModel> Map(DropdownQuestionModel model,
                                    DropdownQuestionPage question,
                                    string actionName,
                                    string controllerName,
                                    IOrderedEnumerable<string> uniqueAwardingOrganisations,
                                    string? selectedAwardingOrganisation,
                                    bool selectedNotOnTheList);
}