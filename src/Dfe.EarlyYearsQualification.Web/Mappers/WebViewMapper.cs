using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class WebViewMapper(IGovUkContentParser contentParser) : IWebViewPageMapper
{
    public async Task<EarlyYearsQualificationListModel> Map(WebViewPage content, WebViewFilters webViewFilters, List<Qualification> qualifications)
    {
        var model = new EarlyYearsQualificationListModel
        {
            Heading = content.Heading,
            PostHeadingContent = await contentParser.ToHtml(content.PostHeadingContent),
            BackButton = NavigationLinkMapper.Map(content.BackButton),
            Qualifications = MapToQualificationModels(qualifications),
            DownloadButtonText = content.DownloadButtonText,
            QualificationLevelLabel = content.QualificationLevelLabel,
            StaffChildRatioLabel = content.StaffChildRatioLabel,
            FromWhichYearLabel = content.FromWhichYearLabel,
            ToWhichYearLabel = content.ToWhichYearLabel,
            AwardingOrganisationLabel = content.AwardingOrganisationLabel,
            QualificationNumberLabel = content.QualificationNumberLabel,
            NotesAdditionalRequirementsLabel = content.NotesAdditionalRequirementsLabel,
            QualificationLevelFilter = webViewFilters.QualificationLevel,
            QualificationStartDateFilter = webViewFilters.QualificationStartDate,
            SearchTermFilter = webViewFilters.SearchTerm,
            NoQualificationsFoundContent = await contentParser.ToHtml(content.NoQualificationsFoundContent),
            CheckIfAnEarlyYearsQualificationIsFullAndRelevantContent = await contentParser.ToHtml(content.QualificationIsFullAndRelevantContent),
            ApplyFiltersButtonContent = content.ApplyFiltersButtonContent,
            ClearFiltersLinkLabel = content.ClearFiltersLinkLabel,
            FilterHeading = content.FilterHeading,
            SelectedFiltersHeading = content.SelectedFiltersHeading,
            KeywordHeading = content.KeywordHeading,
            QualificationStartDateHeading = content.QualificationStartDateHeading,
            QualificationLevelHeading = content.QualificationLevelHeading,
            NoFiltersSelectedContent = content.NoFiltersSelectedContent,
            StartDateFilters = OptionItemMapper.Map(content.StartDateFilters),
            LevelFilters = OptionItemMapper.Map(content.LevelFilters)
        };

        model.ShowingAllQualificationsLabel = model.HasFilters ? $"{qualifications.Count} {(qualifications.Count == 1 ? content.SingleQualificationFoundText : content.MultipleQualificationsFoundText)}" : content.ShowingAllQualificationsLabel;

        return model;
    }

    private static List<QualificationWebViewModel> MapToQualificationModels(List<Qualification> allQualifications)
    {
        return [.. allQualifications.Select(qualification => new QualificationWebViewModel(qualification))];
    }
}