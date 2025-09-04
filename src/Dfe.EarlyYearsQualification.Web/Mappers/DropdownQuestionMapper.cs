using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class DropdownQuestionMapper(IGovUkContentParser contentParser) : IDropdownQuestionMapper
{
    public async Task<DropdownQuestionModel> Map(DropdownQuestionModel model,
                                            DropdownQuestionPage question,
                                            string actionName,
                                            string controllerName,
                                            IOrderedEnumerable<string> uniqueAwardingOrganisations,
                                            string? selectedAwardingOrganisation,
                                            bool selectedNotOnTheList)
    {
        var additionalInformationBodyHtml = await contentParser.ToHtml(question.AdditionalInformationBody);
        
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.CtaButtonText = question.CtaButtonText;
        model.ErrorMessage = question.ErrorMessage;
        model.Question = question.Question;
        model.DropdownHeading = question.DropdownHeading;
        model.NotInListText = question.NotInListText;
        model.BackButton = NavigationLinkMapper.Map(question.BackButton);

        model.Values.Add(new SelectListItem
                         {
                             Text = question.DefaultText,
                             Value = ""
                         });

        foreach (var awardingOrg in uniqueAwardingOrganisations)
        {
            model.Values.Add(new SelectListItem
                             {
                                 Value = awardingOrg,
                                 Text = awardingOrg
                             });
        }

        model.ErrorBannerHeading = question.ErrorBannerHeading;
        model.ErrorBannerLinkText = question.ErrorBannerLinkText;
        model.AdditionalInformationHeader = question.AdditionalInformationHeader;
        model.AdditionalInformationBody = additionalInformationBodyHtml;
        model.NotInTheList = selectedNotOnTheList;
        model.SelectedValue = selectedAwardingOrganisation;
        return model;
    }
}