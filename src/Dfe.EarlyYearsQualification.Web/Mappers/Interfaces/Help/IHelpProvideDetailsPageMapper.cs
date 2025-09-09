using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;

public interface IHelpProvideDetailsPageMapper
{
    ProvideDetailsPageViewModel MapProvideDetailsPageContentToViewModel(HelpProvideDetailsPage content, string reasonForEnquiring);
}