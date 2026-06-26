using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class WebViewMapper(IGovUkContentParser contentParser) : IWebViewPageMapper
{
    private const string MissingValue = "-";
    private const string NoAdditionalRequirements = "None";

    public async Task<EarlyYearsQualificationListModel> Map(WebViewPage content, WebViewFilters webViewFilters, List<Qualification> qualifications)
    {
        var model = new EarlyYearsQualificationListModel
        {
            Heading = content.Heading,
            PostHeadingContent = await contentParser.ToHtml(content.PostHeadingContent),
            BackButton = NavigationLinkMapper.Map(content.BackButton),
            Qualifications = await MapToQualificationModels(qualifications),
            DownloadHeading = content.DownloadHeading,
            DownloadSectionContent = await contentParser.ToHtml(content.DownloadSectionContent),
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
        model.ShowingAllQualificationsLabel = GetShowingAllQualificationsLabel(model.HasFilters, qualifications.Count, content);

        return model;
    }

    private static string GetShowingAllQualificationsLabel(bool hasFilters, int qualificationCount, WebViewPage content)
    {
        if (!hasFilters)
        {
            return content.ShowingAllQualificationsLabel;
        }

        var qualificationText = qualificationCount == 1
            ? content.SingleQualificationFoundText
            : content.MultipleQualificationsFoundText;

        return $"{qualificationCount} {qualificationText}";
    }

    private async Task<List<QualificationWebViewModel>> MapToQualificationModels(List<Qualification> allQualifications)
    {
        return (await Task.WhenAll(allQualifications.Select(MapToQualificationModel))).ToList();
    }

    private async Task<QualificationWebViewModel> MapToQualificationModel(Qualification qualification)
    {
        return new QualificationWebViewModel(qualification)
        {
            FromWhichYear = FormatYearContent(qualification.FromWhichYear),
            ToWhichYear = FormatYearContent(qualification.ToWhichYear),
            StaffChildRatio = qualification.StaffChildRatio,
            QualificationNumber = FormatQualificationNumber(qualification.QualificationNumber),
            EyqlTabs = qualification.EyqlTabs,
            AdditionalRequirements = await GetAdditionalRequirements(qualification),
        };
    }

    private static string FormatQualificationNumber(string? qualificationNumber)
    {
        return StringFormattingHelper.FormatSlashedNumbers(qualificationNumber) ?? MissingValue;
    }

    private async Task<string> GetAdditionalRequirements(Qualification qualification)
    {
        var additionalRequirements = $"{await contentParser.ToHtml(qualification.AdditionalRequirementsRichText)}{qualification.Notes}";

        return string.IsNullOrWhiteSpace(additionalRequirements) ? NoAdditionalRequirements : additionalRequirements;
    }

    private static string FormatYearContent(string? year)
    {
        if (string.IsNullOrEmpty(year) || year == "null")
        {
            return MissingValue;
        }

        var converted = StringDateHelper.ConvertDate(year);
        if (converted.HasValue)
        {
            return StringDateHelper.ConvertToDateString(converted.Value.startMonth, converted.Value.startYear);
        }

        return year;
    }
}