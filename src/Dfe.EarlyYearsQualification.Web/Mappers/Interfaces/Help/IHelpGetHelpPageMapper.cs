using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;

public interface IHelpGetHelpPageMapper
{
    Task<GetHelpPageViewModel> MapGetHelpPageContentToViewModelAsync(GetHelpPage helpPageContent);
}