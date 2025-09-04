using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IAccessibilityStatementMapper
{
    Task<AccessibilityStatementPageModel> Map(AccessibilityStatementPage content);
}